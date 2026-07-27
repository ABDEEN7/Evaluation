using System.Drawing;
using System.Net.Mail;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using Evaluation.Services.Special;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Evaluation.DAL.Dtos.Form;
using Evaluation.SharedHelper.Dtos.Form;
using Xceed.Document.NET;
using Xceed.Words.NET;
using Evaluation.DAL.Models.FormBuilder;
using Evaluation.DAL.Models.Website;
using Evaluation.DAL.Repositories;
using System.Globalization;

namespace Evaluation.Services.BusinessLayer.API.Template;

public class TemplateService(
    UnitOfWork uow,
    RequestInfo requestInfo,
    AzureBlobStorageService blobService,
    IServiceProvider serviceProvider,
    PlaceholderService placeholderService,
    CacheDataProvider cacheDataProvider)
    : ApiServiceBase
{
    private const string ReportFont = "Lusail";

    private static readonly Color ReportMaroon =
        Color.FromArgb(115, 0, 57); // #730039

    private static readonly Color ReportFooterGray =
        Color.FromArgb(217, 217, 217);

    private const string Rlm = "\u200F";
    private const string Lrm = "\u200E";

    private static readonly XNamespace W =
        "http://schemas.openxmlformats.org/wordprocessingml/2006/main";


    public string GetTextFromHtml(string template, List<PlaceholderDto> placeholders)
    {
        if (string.IsNullOrEmpty(template)) return string.Empty;

        try
        {
            return placeholders.Aggregate(template, (current, placeholder) =>
                placeholder.Key != null ? current.Replace(placeholder.Key, placeholder.Value) : current);
        }
        catch (Exception ex)
        {
            throw new BusinessException($"An error occurred while replacing placeholders: {ex.Message}");
        }
    }

    public async Task<byte[]> GenerateAttachments(Guid templateId, List<PlaceholderDto> placeholders,
        Guid systemModuleId)
    {
        var scopedUow = serviceProvider.CreateScopedUow();
        var attachment = await scopedUow.GetRepository<WebsiteAttachment>().GetByIDActiveNonDeleted(templateId);

        if (attachment == null)
            throw new BusinessException(ConstantKeys.ExceptionMessage.InActiveData);

        return await HandleAttachment(placeholders, attachment.Id, systemModuleId);
    }

    public async Task<Attachment> HandleAttachment(Guid attachmentId)
    {
        var attachment = await serviceProvider.CreateScopedUow()
            .GetRepository<WebsiteAttachment>()
            .GetByIDActiveNonDeleted(attachmentId);

        if (attachment == null)
            throw new BusinessException(ConstantKeys.ExceptionMessage.InActiveData);

        var url = blobService.GenerateSasToken(attachment.FileName);
        using var httpClient = new HttpClient();
        var fileBytes = await httpClient.GetByteArrayAsync(url);
        var memoryStream = new MemoryStream(fileBytes) { Position = 0 };
        return new Attachment(memoryStream, attachment.FileName);
    }

    public async Task<byte[]> HandleAttachment(List<PlaceholderDto> placeholders, Guid attachmentId,
        Guid systemModuleId)
    {
        var attachment = await serviceProvider.CreateScopedUow().GetRepository<WebsiteAttachment>()
            .GetByIDActiveNonDeleted(attachmentId);
        if (attachment == null)
            throw new BusinessException(ConstantKeys.ExceptionMessage.InActiveData);

        var widthSetting =
            await cacheDataProvider.GetSystemSettingValue(ConstantKeys.AdminSettings.SignatureUploadWidth);
        var heightSetting =
            await cacheDataProvider.GetSystemSettingValue(ConstantKeys.AdminSettings.SignatureUploadHeight);

        int.TryParse(widthSetting, out var width);
        int.TryParse(heightSetting, out var height);

        var url = blobService.GenerateSasToken(attachment.FileName, 0, null, false, StorageContainerType.website);
        using var httpClient = new HttpClient();
        var fileBytes = await httpClient.GetByteArrayAsync(url);

        using var stream = new MemoryStream(fileBytes);
        using var docx = Xceed.Words.NET.DocX.Load(stream);
        await ReplacePlaceholders(docx, placeholders, width, height);

        using var output = new MemoryStream();
        docx.SaveAs(output);
        return output.ToArray();
    }

    private async Task ReplacePlaceholders(DocX docx, List<PlaceholderDto> placeholders, int maxWidth, int maxHeight)
    {
        foreach (var placeholder in placeholders.Where(placeholder => !string.IsNullOrWhiteSpace(placeholder.Key)))
        {
            switch (placeholder.PlaceholderType)
            {
                case PlaceholderType.Text:
                case PlaceholderType.Untyped:
                    var textKey = NormalizePlaceholderKey(placeholder.Key); // Remove curly braces
                    docx.ReplaceText(new StringReplaceTextOptions
                    {
                        SearchValue = $"{{{{{textKey}}}}}",
                        NewValue = placeholder.Value
                    });
                    break;

                case PlaceholderType.Image:
                    var imageKey = NormalizePlaceholderKey(placeholder.Key);

                    if (!string.IsNullOrWhiteSpace(placeholder.Value))
                    {
                        try
                        {
                            var imageData = Convert.FromBase64String(placeholder.Value);

                            // Keep stream alive
                            using var ms = new MemoryStream(imageData);

                            var image = docx.AddImage(ms); // AddImage does NOT make a deep copy
                            var picture = image.CreatePicture(maxHeight, maxWidth);

                            docx.ReplaceTextWithObject(new ObjectReplaceTextOptions
                            {
                                SearchValue = $"{{{{{imageKey}}}}}",
                                NewObject = picture,
                                TrackChanges = false
                            });

                            // Don't dispose ms here until everything is done with image
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(
                                $"Image replacement failed for placeholder '{placeholder.Key}': {ex.Message}");
                        }
                    }

                    break;
                case PlaceholderType.HTMLTable:
                    var htmlTableKey = NormalizePlaceholderKey(placeholder.Key); // Remove curly braces
                    if (!string.IsNullOrWhiteSpace(placeholder.Value))
                    {
                        var data = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(placeholder.Value) ??
                                   [];
                        var childrenFieldIds = placeholders
                            .FirstOrDefault(p => NormalizePlaceholderKey(p.Key) == htmlTableKey)?
                            .ChildFieldId?.Split(',').Select(Guid.Parse).ToList() ?? [];
                        var childFields = await uow.GetRepository<Field>().GetAllActiveNonDeleted()
                            .Include(f => f.FieldType)
                            .Where(f => childrenFieldIds.Contains(f.Id))
                            .ToListAsync();
                        childFields.Reverse();
                        var docxTable = docx.AddTable(data.Count + 1, childFields.Count); // +1 for the header row
                        var colIndex = 0;
                        foreach (var column in childFields)
                        {
                            docxTable.Rows[0].Cells[colIndex].Paragraphs.First().Append(column.TitleAr);
                            colIndex++;
                        }

                        for (var rowIndex = 0; rowIndex < data.Count; rowIndex++)
                        {
                            colIndex = 0;
                            foreach (var field in childFields)
                            {
                                data[rowIndex].TryGetValue(field.Id.ToString(), out var value);
                                var finalValue = await placeholderService.RetrieveValueAsync(
                                    field.FieldType!.BackendName,
                                    field.DropDownTypeId, $"{value}", requestInfo.Lang, htmlTableKey);
                                docxTable.Rows[rowIndex + 1].Cells[colIndex].Paragraphs.First().Append(finalValue);
                                colIndex++;
                            }
                        }

                        docxTable.Design = TableDesign.LightShading;
                        docxTable.AutoFit = AutoFit.Contents;
                        docx.ReplaceTextWithObject(new ObjectReplaceTextOptions
                        {
                            SearchValue = $"{{{{{htmlTableKey}}}}}",
                            NewObject = docxTable,
                            TrackChanges = false
                        });
                    }

                    break;
                case PlaceholderType.Table:
                    var tablePlaceholders = placeholders
                        .Where(x => x.PlaceholderType == PlaceholderType.Table)
                        .ToList();
                    ProcessTablePlaceholders(docx, tablePlaceholders);
                    break;
                case PlaceholderType.SchoolPeriodicEvaluationCriteria:
                    await ProcessSchoolPeriodicEvaluationCriteriaPlaceholderAsync(docx, placeholder);
                    break;
                case PlaceholderType.SchoolPerformanceSummaryTable:
                {
                    var key = NormalizePlaceholderKey(placeholder.Key);

                    if (placeholder.SchoolPerformanceResult is { Count: > 0 })
                    {
                        var table = CreateSchoolPerformanceSummaryTable(docx, placeholder.SchoolPerformanceResult);

                        docx.ReplaceTextWithObject(new ObjectReplaceTextOptions
                        {
                            SearchValue = $"{{{{{key}}}}}",
                            NewObject = table,
                            TrackChanges = false
                        });
                    }

                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(placeholder.PlaceholderType),
                        $"Unsupported placeholder type: {placeholder.PlaceholderType}");
            }
        }
    }

    private Task ProcessSchoolPeriodicEvaluationCriteriaPlaceholderAsync(
        DocX docx,
        PlaceholderDto placeholder)
    {
        var key = $"{{{{{NormalizePlaceholderKey(placeholder.Key)}}}}}";

        var anchor = docx.Paragraphs
            .FirstOrDefault(p => p.Text.Contains(key, StringComparison.Ordinal));

        if (anchor == null)
            throw new BusinessException(
                $"The placeholder {key} was not found in the document.");

        if (!string.Equals(anchor.Text.Trim(), key, StringComparison.Ordinal))
            throw new BusinessException(
                $"The placeholder {key} must be alone in its paragraph.");

        var rootIndex = 0;

        foreach (var root in OrderedScopes(placeholder.Tree))
        {
            rootIndex++;

            InsertRootScope(
                anchor,
                root,
                Segment(root.OrderNo, rootIndex));
        }

        anchor.Remove(false);

        return Task.CompletedTask;
    }

    private static string NormalizePlaceholderKey(string? key) => (key ?? string.Empty).Trim().Trim('{', '}');

    private static IEnumerable<ScopeTreeDto> OrderedScopes(IEnumerable<ScopeTreeDto>? scopes) =>
        (scopes ?? []).Select((x, i) => (Scope: x, Index: i + 1))
        .OrderBy(x => ValidOrder(x.Scope.OrderNo) ? x.Scope.OrderNo : x.Index).Select(x => x.Scope);

    private static IEnumerable<FormItemDto> OrderedItems(IEnumerable<FormItemDto>? items) =>
        (items ?? []).Select((x, i) => (Item: x, Index: i + 1))
        .OrderBy(x => ValidOrder(x.Item.OrderNo) ? x.Item.OrderNo : x.Index).Select(x => x.Item);

    private static bool ValidOrder(int orderNo) => orderNo > 0;
    private static string Segment(int orderNo, int index) => (ValidOrder(orderNo) ? orderNo : index).ToString();

    private void InsertRootScope(
        Paragraph anchor,
        ScopeTreeDto scope,
        string number)
    {
        var ordinal = ArabicOrdinal(
            int.TryParse(number, out var n) ? n : 1);

        AddArabicParagraphBefore(
            anchor,
            $"المعيار  {ordinal}: {scope.Name}",
            size: 14,
            bold: true,
            alignment: Alignment.center,
            keepNext: true,
            after: 0);

        AddArabicParagraphBefore(
            anchor,
            $"مستوى {scope.Name} \" {GetJudgement(GetScopeAverage(scope))} \"",
            size: 13,
            bold: true,
            alignment: Alignment.center,
            keepNext: true,
            after: 4);

        var itemIndex = 0;

        foreach (var item in OrderedItems(scope.Items))
        {
            InsertItem(
                anchor,
                item,
                $"{number}.{Segment(item.OrderNo, ++itemIndex)}");
        }

        var childIndex = 0;

        foreach (var child in OrderedScopes(scope.Children))
        {
            InsertScopeWithNumber(
                anchor,
                child,
                $"{number}.{Segment(child.OrderNo, ++childIndex)}",
                depth: 1);
        }

        // Strengths and weaknesses belong to the main criterion only.
        InsertStrengthsAndWeaknesses(anchor, scope);
    }
    
    private void InsertStrengthsAndWeaknesses(
        Paragraph anchor,
        ScopeTreeDto scope)
    {
        var strengths = scope.Strengths?
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim())
            .ToList() ?? [];

        var weaknesses = scope.Weaknesses?
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim())
            .ToList() ?? [];

        if (strengths.Count > 0)
        {
            InsertEvaluationPointsSection(
                anchor,
                "أهم جوانب القوة",
                strengths);
        }

        if (weaknesses.Count > 0)
        {
            InsertEvaluationPointsSection(
                anchor,
                "أهم الجوانب التي تحتاج إلى تحسين وتطوير",
                weaknesses);
        }
    }
    
    private void InsertEvaluationPointsSection(
        Paragraph anchor,
        string title,
        IEnumerable<string> items)
    {
        var values = items
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();

        if (values.Count == 0)
            return;

        // Maroon section header
        AddArabicParagraphBefore(
            anchor,
            title,
            size: 12.5,
            bold: true,
            alignment: Alignment.center,
            keepNext: true,
            textColor: Color.White,
            backgroundColor: ReportMaroon,
            before: 5,
            after: 2);

        // Section items
        foreach (var item in values)
        {
            AddArabicParagraphBefore(
                anchor,
                $"▪ {item}",
                size: 11.5,
                bold: false,
                alignment: Alignment.both,
                after: 2);
        }
    }

    private void InsertScopeWithNumber(
        Paragraph anchor,
        ScopeTreeDto scope,
        string number,
        int depth)
    {
        if (depth == 1)
        {
            AddArabicParagraphBefore(
                anchor,
                $"{number} {scope.Name}",
                size: 12.5,
                bold: true,
                alignment: Alignment.center,
                keepNext: true,
                textColor: Color.White,
                backgroundColor: ReportMaroon,
                before: 3,
                after: 1);
        }
        else
        {
            AddArabicParagraphBefore(
                anchor,
                $"{number} {scope.Name}",
                size: 12,
                bold: true,
                alignment: Alignment.center,
                keepNext: true,
                before: 2,
                after: 0);
        }

        var itemIndex = 0;

        foreach (var item in OrderedItems(scope.Items))
        {
            InsertItem(
                anchor,
                item,
                $"{number}.{Segment(item.OrderNo, ++itemIndex)}");
        }

        var childIndex = 0;

        foreach (var child in OrderedScopes(scope.Children))
        {
            InsertScopeWithNumber(
                anchor,
                child,
                $"{number}.{Segment(child.OrderNo, ++childIndex)}",
                depth + 1);
        }
    }

    private void InsertItem(
        Paragraph anchor,
        FormItemDto item,
        string number)
    {
        // Example:
        // 1.1.1 الرؤية والرسالة
        AddArabicParagraphBefore(
            anchor,
            $"{number} {item.Name}",
            size: 12,
            bold: true,
            alignment: Alignment.both,
            keepNext: true,
            before: 2,
            after: 0);

        if (!string.IsNullOrWhiteSpace(item.Note))
        {
            AddArabicParagraphBefore(
                anchor,
                $"▪ {item.Note}",
                size: 11.5,
                bold: false,
                alignment: Alignment.both,
                after: 2);
        }

        foreach (var related in item.RelatedItems ?? [])
        {
            if (string.IsNullOrWhiteSpace(related.Note))
                continue;

            AddArabicParagraphBefore(
                anchor,
                $"▪ {related.Note}",
                size: 11.5,
                bold: false,
                alignment: Alignment.both,
                after: 2);
        }
    }

    private Paragraph AddArabicParagraphBefore(
        Paragraph anchor,
        string text,
        double size,
        bool bold,
        Alignment alignment,
        bool keepNext = false,
        Color? textColor = null,
        Color? backgroundColor = null,
        double before = 0,
        double after = 0)
    {
        // RLM helps Word correctly position numbers such as
        // "1.1.1" beside Arabic text.
        var paragraph = anchor.InsertParagraphBeforeSelf(
            $"{Rlm}{text}");

        paragraph.Alignment = alignment;

        ApplyArabicParagraphStyle(
            paragraph,
            size,
            bold,
            keepNext,
            textColor ?? Color.Black,
            before,
            after);

        if (backgroundColor.HasValue)
        {
            ApplyParagraphShading(
                paragraph,
                backgroundColor.Value);
        }

        return paragraph;
    }

    private static void ApplyArabicParagraphStyle(
        Paragraph paragraph,
        double fontSize,
        bool bold,
        bool keepNext,
        Color textColor,
        double before,
        double after)
    {
        var pPr = paragraph.Xml.Element(W + "pPr");

        if (pPr == null)
        {
            pPr = new XElement(W + "pPr");
            paragraph.Xml.AddFirst(pPr);
        }

        SetToggle(pPr, "bidi", true);
        SetToggle(pPr, "keepLines", true);
        SetToggle(pPr, "widowControl", true);

        if (keepNext)
            SetToggle(pPr, "keepNext", true);

        SetParagraphSpacing(
            pPr,
            before,
            after);

        // IMPORTANT:
        // Apply the Arabic formatting to EVERY run,
        // not just the first run.
        foreach (var run in paragraph.Xml.Descendants(W + "r"))
        {
            ApplyArabicRunStyle(
                run,
                fontSize,
                bold,
                textColor);
        }
    }

    private static void ApplyArabicRunStyle(
        XElement run,
        double fontSize,
        bool bold,
        Color textColor)
    {
        var rPr = run.Element(W + "rPr");

        if (rPr == null)
        {
            rPr = new XElement(W + "rPr");
            run.AddFirst(rPr);
        }

        // -----------------------------
        // Font
        // -----------------------------

        var fonts = rPr.Element(W + "rFonts");

        if (fonts == null)
        {
            fonts = new XElement(W + "rFonts");
            rPr.AddFirst(fonts);
        }

        fonts.SetAttributeValue(W + "ascii", ReportFont);
        fonts.SetAttributeValue(W + "hAnsi", ReportFont);
        fonts.SetAttributeValue(W + "eastAsia", ReportFont);

        // THIS is what matters for Arabic
        fonts.SetAttributeValue(W + "cs", ReportFont);
        fonts.SetAttributeValue(W + "hint", "cs");

        // -----------------------------
        // Arabic / complex script
        // -----------------------------

        SetToggle(rPr, "rtl", true);
        SetToggle(rPr, "cs", true);

        // -----------------------------
        // Font size
        // -----------------------------

        var halfPoints = Math.Round(fontSize * 2)
            .ToString(CultureInfo.InvariantCulture);

        SetValue(rPr, "sz", halfPoints);

        // Arabic font size
        SetValue(rPr, "szCs", halfPoints);

        // -----------------------------
        // Bold
        // -----------------------------

        SetToggle(rPr, "b", bold);

        // Arabic bold
        SetToggle(rPr, "bCs", bold);

        // -----------------------------
        // Language
        // -----------------------------

        var lang = rPr.Element(W + "lang");

        if (lang == null)
        {
            lang = new XElement(W + "lang");
            rPr.Add(lang);
        }

        lang.SetAttributeValue(W + "val", "ar-QA");
        lang.SetAttributeValue(W + "bidi", "ar-QA");

        // -----------------------------
        // Color
        // -----------------------------

        SetValue(
            rPr,
            "color",
            $"{textColor.R:X2}{textColor.G:X2}{textColor.B:X2}");
    }

    private static void SetParagraphSpacing(
        XElement pPr,
        double before,
        double after)
    {
        var spacing = pPr.Element(W + "spacing");

        if (spacing == null)
        {
            spacing = new XElement(W + "spacing");
            pPr.Add(spacing);
        }

        spacing.SetAttributeValue(
            W + "before",
            ToTwips(before));

        spacing.SetAttributeValue(
            W + "after",
            ToTwips(after));

        // Single line spacing.
        // Keeps the hierarchy compact like the reference report.
        spacing.SetAttributeValue(W + "line", "240");
        spacing.SetAttributeValue(W + "lineRule", "auto");
    }

    private static string ToTwips(double points) =>
        Math.Round(points * 20)
            .ToString(CultureInfo.InvariantCulture);

    private static void SetToggle(
        XElement parent,
        string elementName,
        bool enabled)
    {
        var element = parent.Element(W + elementName);

        if (element == null)
        {
            element = new XElement(W + elementName);
            parent.Add(element);
        }

        element.SetAttributeValue(
            W + "val",
            enabled ? "1" : "0");
    }

    private static void SetValue(
        XElement parent,
        string elementName,
        string value)
    {
        var element = parent.Element(W + elementName);

        if (element == null)
        {
            element = new XElement(W + elementName);
            parent.Add(element);
        }

        element.SetAttributeValue(W + "val", value);
    }

    private static void ApplyParagraphShading(
        Paragraph paragraph,
        Color color)
    {
        var pPr = paragraph.Xml.Element(W + "pPr");

        if (pPr == null)
        {
            pPr = new XElement(W + "pPr");
            paragraph.Xml.AddFirst(pPr);
        }

        pPr.Elements(W + "shd").Remove();

        pPr.Add(
            new XElement(
                W + "shd",
                new XAttribute(W + "val", "clear"),
                new XAttribute(W + "color", "auto"),
                new XAttribute(
                    W + "fill",
                    $"{color.R:X2}{color.G:X2}{color.B:X2}")));
    }

    private Paragraph AddBefore(Paragraph anchor, string text, string font, double size, bool bold, Alignment alignment,
        bool keepNext = false)
    {
        var p = anchor.InsertParagraphBeforeSelf(text);
        p.Font(font).FontSize(size).Alignment = alignment;
        if (bold) p.Bold();
        ApplyRtlProperties(p, keepNext);
        return p;
    }

    private static void ApplyRtlProperties(Paragraph p, bool keepNext)
    {
        XNamespace w = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";
        var pPr = p.Xml.Element(w + "pPr") ?? new XElement(w + "pPr");
        if (pPr.Parent == null) p.Xml.AddFirst(pPr);

        void Add(string name)
        {
            if (pPr.Element(w + name) == null) pPr.Add(new XElement(w + name));
        }

        Add("bidi");
        Add("keepLines");
        Add("widowControl");
        if (keepNext) Add("keepNext");
        var rPr = p.Xml.Element(w + "r")?.Element(w + "rPr");
        if (rPr == null) return;
        if (rPr.Element(w + "rtl") == null) rPr.Add(new XElement(w + "rtl"));
        if (rPr.Element(w + "cs") == null) rPr.Add(new XElement(w + "cs"));
    }

    private static void ApplyShading(Paragraph p, Color color)
    {
        XNamespace w = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";
        var pPr = p.Xml.Element(w + "pPr") ?? new XElement(w + "pPr");
        if (pPr.Parent == null) p.Xml.AddFirst(pPr);
        pPr.Add(new XElement(w + "shd", new XAttribute(w + "fill", $"{color.R:X2}{color.G:X2}{color.B:X2}")));
        p.Color(Xceed.Drawing.Color.White);
    }

    private static string ArabicOrdinal(int n) => n switch
    {
        1 => "الأول", 2 => "الثاني", 3 => "الثالث", 4 => "الرابع", 5 => "الخامس", 6 => "السادس", 7 => "السابع",
        8 => "الثامن", 9 => "التاسع", 10 => "العاشر", _ => n.ToString()
    };

    private static string GetTemplateFont(DocX docx) => "Lusail";

    private decimal GetScopeAverage(ScopeTreeDto scope)
    {
        var items = GetAllItems(scope);
        return items.Where(x => x.Value.HasValue).Select(x => x.Value!.Value).DefaultIfEmpty(0).Average();
    }

    private static List<FormItemDto> GetAllItems(ScopeTreeDto scope)
    {
        var result = new List<FormItemDto>();
        result.AddRange(scope.Items ?? []);
        foreach (var child in scope.Children ?? []) result.AddRange(GetAllItems(child));
        return result;
    }

    private static string GetJudgement(decimal value) => value switch
    {
        < 3m => "ضعيف", < 3.75m => "مقبول", < 4.25m => "جيد", < 4.75m => "جيد جداً", _ => "ممتاز"
    };

    private void ProcessTablePlaceholders(DocX docx, List<PlaceholderDto> tablePlaceholders)
    {
        if (tablePlaceholders.Count == 0) return;

        foreach (var table in docx.Tables.Where(t => t.RowCount > 0))
        {
            ProcessTableAsync(table, tablePlaceholders);
        }
    }

    private void ProcessTableAsync(Table table, List<PlaceholderDto> tablePlaceholders)
    {
        if (table.Rows.Count < 2) return; // Ensure there is at least one template row
        var templateRow = table.Rows[1];
        var placeholderKeys = ExtractPlaceholderKeysFromRow(templateRow);

        if (placeholderKeys.Count == 0) return;

        var relevantPlaceholders = GetRelevantPlaceholders(tablePlaceholders, placeholderKeys);
        if (relevantPlaceholders.Count == 0) return;

        var placeholderData = PreparePlaceholderData(relevantPlaceholders);
        var rowCount = placeholderData.First().Value.Rows.Count;

        AddDataRowsToTable(table, templateRow, placeholderKeys, placeholderData, rowCount);

        table.RemoveRow(1); // remove template row
    }

    private HashSet<string> ExtractPlaceholderKeysFromRow(Row row)
    {
        var placeholderKeys = new HashSet<string>();

        foreach (var match in from cell in row.Cells
                 from para in cell.Paragraphs
                 select Regex.Matches(para.Text, @"{{(.*?)}}")
                 into matches
                 from Match match in matches
                 select match)
        {
            placeholderKeys.Add(match.Value);
        }

        return placeholderKeys;
    }

    private List<PlaceholderDto> GetRelevantPlaceholders(List<PlaceholderDto> allPlaceholders,
        HashSet<string> placeholderKeys)
    {
        return allPlaceholders
            .Where(p => !string.IsNullOrEmpty(p.Key) && placeholderKeys.Contains(p.Key))
            .ToList();
    }

    private Dictionary<string, RowChildData> PreparePlaceholderData(List<PlaceholderDto> relevantPlaceholders)
    {
        var placeholderData = new Dictionary<string, RowChildData>();

        foreach (var placeholder in relevantPlaceholders
                     .Where(x => !string.IsNullOrEmpty(x.Key) && !string.IsNullOrEmpty(x.ChildFieldId)))
        {
            var rows = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(placeholder.Value ?? "[]") ?? [];
            placeholderData[placeholder.Key!] =
                new RowChildData(Guid.Parse(placeholder.ChildFieldId!), placeholder.ChildField, rows);
        }

        return placeholderData;
    }

    private void AddDataRowsToTable(Table table, Row templateRow,
        HashSet<string> placeholderKeys, Dictionary<string, RowChildData> placeholderData, int rowCount)
    {
        for (var index = 0; index < rowCount; index++)
        {
            var newRow = table.InsertRow(templateRow);
            ProcessRowPlaceholders(newRow, placeholderKeys, placeholderData, index);
        }
    }

    private void ProcessRowPlaceholders(Row row, HashSet<string> placeholderKeys,
        Dictionary<string, RowChildData> placeholderData, int rowIndex)
    {
        foreach (var para in row.Cells.SelectMany(cell => cell.Paragraphs))
        {
            ReplacePlaceholdersInParagraph(para, placeholderKeys, placeholderData, rowIndex);
        }
    }

    private void ReplacePlaceholdersInParagraph(Paragraph para, HashSet<string> placeholderKeys,
        Dictionary<string, RowChildData> placeholderData, int rowIndex)
    {
        foreach (var key in placeholderKeys.Where(placeholderData.ContainsKey))
        {
            var (childFieldId, childField, rowDataList) = placeholderData[key];
            var rowData = rowDataList[rowIndex];

            if (childFieldId == null || childField == null) continue;
            if (!rowData.TryGetValue($"{childFieldId}", out var rawValue)) continue;

            if (!para.Text.Contains(key)) continue;
            var updatedText = para.Text.Replace(key, rawValue);
            para.RemoveText(0);
            para.Append(updatedText);
        }
    }

    private static Table CreateSchoolPerformanceSummaryTable(
        DocX docx,
        List<SchoolPerformanceResult> data)
    {
        var table = docx.AddTable(
            data.Count + 2,
            4);

        table.Design = TableDesign.TableGrid;
        table.AutoFit = AutoFit.Window;
        table.Alignment = Alignment.center;

        string[] headers =
        {
            "نسبة المعيار الرئيس",
            "حكم المعيار الرئيس",
            "الوزن النسبي للمعيار الرئيس",
            "المعايير الرئيسة"
        };

        // -----------------------
        // Header
        // -----------------------

        for (var i = 0; i < headers.Length; i++)
        {
            var cell = table.Rows[0].Cells[i];

            SetCellText(
                cell,
                headers[i],
                fontSize: 12.5,
                bold: true,
                textColor: Color.White);

            SetCellShading(
                cell,
                ReportMaroon);
        }

        SetRowMinimumHeight(
            table.Rows[0],
            36);

        // -----------------------
        // Data
        // -----------------------

        for (var rowIndex = 0;
             rowIndex < data.Count;
             rowIndex++)
        {
            var item = data[rowIndex];
            var row = table.Rows[rowIndex + 1];

            SetCellText(
                row.Cells[0],
                FormatPercentage(item.Average));

            SetCellText(
                row.Cells[1],
                item.CriteriaJudgement);

            SetCellText(
                row.Cells[2],
                FormatPercentage(item.CriteriaWieght));

            SetCellText(
                row.Cells[3],
                item.CriteriaName);

            SetRowMinimumHeight(
                row,
                28);
        }

        // -----------------------
        // Overall
        // -----------------------

        var overallAverage =
            CalculateOverallAverage(data);

        var overallJudgement =
            GetJudgement(overallAverage / 20);

        var footer =
            table.Rows[data.Count + 1];

        SetCellText(
            footer.Cells[0],
            FormatPercentage(overallAverage));

        SetCellText(
            footer.Cells[1],
            overallJudgement);

        footer.MergeCells(2, 3);

        SetCellText(
            footer.Cells[2],
            "الحكم العام");

        foreach (var cell in footer.Cells)
        {
            SetCellShading(
                cell,
                ReportFooterGray);
        }

        SetRowMinimumHeight(
            footer,
            28);

        return table;
    }

    private static string FormatPercentage(decimal value)
    {
        return $"{Lrm}{Math.Round(value):0}%{Lrm}";
    }

    private static string FormatPercentage(int value)
    {
        return $"{Lrm}{value}%{Lrm}";
    }

    private static void SetRowMinimumHeight(
        Row row,
        double points)
    {
        var trPr =
            row.Xml.Element(W + "trPr");

        if (trPr == null)
        {
            trPr = new XElement(W + "trPr");
            row.Xml.AddFirst(trPr);
        }

        trPr.Elements(W + "trHeight").Remove();

        trPr.Add(
            new XElement(
                W + "trHeight",
                new XAttribute(
                    W + "val",
                    ToTwips(points)),
                new XAttribute(
                    W + "hRule",
                    "atLeast")));
    }

    private static void SetCellText(
        Cell cell,
        string? text,
        double fontSize = 12,
        bool bold = true,
        Color? textColor = null)
    {
        var paragraph =
            cell.Paragraphs.First();

        paragraph.RemoveText(0);

        paragraph.Append(
            text ?? string.Empty);

        paragraph.Alignment =
            Alignment.center;

        ApplyArabicParagraphStyle(
            paragraph,
            fontSize,
            bold,
            keepNext: false,
            textColor ?? Color.Black,
            before: 0,
            after: 0);

        ApplyCellLayout(cell);
    }

    private static void ApplyCellLayout(Cell cell)
    {
        var tcPr = cell.Xml.Element(W + "tcPr");

        if (tcPr == null)
        {
            tcPr = new XElement(W + "tcPr");
            cell.Xml.AddFirst(tcPr);
        }

        var verticalAlignment =
            tcPr.Element(W + "vAlign");

        if (verticalAlignment == null)
        {
            verticalAlignment =
                new XElement(W + "vAlign");

            tcPr.Add(verticalAlignment);
        }

        verticalAlignment.SetAttributeValue(
            W + "val",
            "center");

        var margins =
            tcPr.Element(W + "tcMar");

        if (margins == null)
        {
            margins = new XElement(W + "tcMar");
            tcPr.Add(margins);
        }

        SetCellMargin(margins, "top", 60);
        SetCellMargin(margins, "bottom", 60);
        SetCellMargin(margins, "left", 60);
        SetCellMargin(margins, "right", 60);
    }

    private static void SetCellMargin(
        XElement margins,
        string side,
        int twips)
    {
        var element =
            margins.Element(W + side);

        if (element == null)
        {
            element = new XElement(W + side);
            margins.Add(element);
        }

        element.SetAttributeValue(
            W + "w",
            twips);

        element.SetAttributeValue(
            W + "type",
            "dxa");
    }

    private static void SetCellShading(
        Cell cell,
        Color color)
    {
        var tcPr =
            cell.Xml.Element(W + "tcPr");

        if (tcPr == null)
        {
            tcPr = new XElement(W + "tcPr");
            cell.Xml.AddFirst(tcPr);
        }

        tcPr.Elements(W + "shd").Remove();

        tcPr.Add(
            new XElement(
                W + "shd",
                new XAttribute(W + "val", "clear"),
                new XAttribute(W + "color", "auto"),
                new XAttribute(
                    W + "fill",
                    $"{color.R:X2}{color.G:X2}{color.B:X2}")));
    }

    private static void SetParagraphRtl(Paragraph p)
    {
        XNamespace w = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";

        var pPr = p.Xml.Element(w + "pPr");
        if (pPr == null)
        {
            pPr = new XElement(w + "pPr");
            p.Xml.AddFirst(pPr);
        }

        if (pPr.Element(w + "bidi") == null)
        {
            pPr.Add(new XElement(w + "bidi"));
        }
    }

    private static decimal CalculateOverallAverage(List<SchoolPerformanceResult> data)
    {
        var totalWeight = data.Sum(x => x.CriteriaWieght);

        if (totalWeight > 0)
        {
            return data.Sum(x => x.Average * x.CriteriaWieght) / totalWeight;
        }

        return data.Average(x => x.Average);
    }
}