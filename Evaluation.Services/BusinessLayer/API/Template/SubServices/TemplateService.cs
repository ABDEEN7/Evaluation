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
using Evaluation.Services.BusinessLayer.API.FormLayer;
using Evaluation.Services.BusinessLayer.API.Template;
using Evaluation.SharedHelper.Dtos.Form;
using Xceed.Document.NET;
using Xceed.Words.NET;
using Evaluation.DAL.Models.FormBuilder;
using Evaluation.DAL.Models.Website;
using Evaluation.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.API.Template;


	public class TemplateService(UnitOfWork uow, RequestInfo requestInfo,
        AzureBlobStorageService blobService, IServiceProvider serviceProvider, PlaceholderService placeholderService,
        CacheDataProvider cacheDataProvider)
        : ApiServiceBase
    {
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

        public async Task<byte[]> GenerateAttachments(Guid templateId, List<PlaceholderDto> placeholders, Guid systemModuleId, SchoolPeriodicEvaluationCriteriaContext? criteriaContext = null)
        {
            var scopedUow = serviceProvider.CreateScopedUow();
            var attachment = await scopedUow.GetRepository<WebsiteAttachment>().GetByIDActiveNonDeleted(templateId);

            if (attachment == null)
                throw new BusinessException(ConstantKeys.ExceptionMessage.InActiveData);

            return await HandleAttachment(placeholders, attachment.Id, systemModuleId, criteriaContext);
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
        
        public async Task<byte[]> HandleAttachment(List<PlaceholderDto> placeholders, Guid attachmentId, Guid systemModuleId, SchoolPeriodicEvaluationCriteriaContext? criteriaContext = null)
        {
            var attachment = await serviceProvider.CreateScopedUow().GetRepository<WebsiteAttachment>().GetByIDActiveNonDeleted(attachmentId);
            if (attachment == null)
                throw new BusinessException(ConstantKeys.ExceptionMessage.InActiveData);

            var widthSetting = await cacheDataProvider.GetSystemSettingValue(ConstantKeys.AdminSettings.SignatureUploadWidth);
            var heightSetting = await cacheDataProvider.GetSystemSettingValue(ConstantKeys.AdminSettings.SignatureUploadHeight);

            int.TryParse(widthSetting, out var width);
            int.TryParse(heightSetting, out var height);

            var url = blobService.GenerateSasToken(attachment.FileName,0,null,false, StorageContainerType.website);
            using var httpClient = new HttpClient();
            var fileBytes = await httpClient.GetByteArrayAsync(url);

            using var stream = new MemoryStream(fileBytes);
            using var docx = Xceed.Words.NET.DocX.Load(stream);
            await ReplacePlaceholders(docx, placeholders, width, height, criteriaContext);

            using var output = new MemoryStream();
            docx.SaveAs(output);
            return output.ToArray();
        }

        private async Task ReplacePlaceholders(DocX docx, List<PlaceholderDto> placeholders, int maxWidth, int maxHeight, SchoolPeriodicEvaluationCriteriaContext? criteriaContext)
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
                            Console.WriteLine($"Image replacement failed for placeholder '{placeholder.Key}': {ex.Message}");
                        }
                    }
                    break;
                case PlaceholderType.HTMLTable:
                        var htmlTableKey = NormalizePlaceholderKey(placeholder.Key); // Remove curly braces
                        if (!string.IsNullOrWhiteSpace(placeholder.Value))
                        {
                            var data = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(placeholder.Value) ?? [];
                            var childrenFieldIds = placeholders.FirstOrDefault(p => NormalizePlaceholderKey(p.Key) == htmlTableKey)?
                                .ChildFieldId?.Split(',').Select(Guid.Parse).ToList() ?? [];
                            var childFields = await uow.GetRepository<Field>().GetAllActiveNonDeleted()
                                .Include(f=>f.FieldType)
                                .Where(f=> childrenFieldIds.Contains(f.Id))
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
                                    var finalValue = await placeholderService.RetrieveValueAsync(field.FieldType!.BackendName,
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
                        await ProcessSchoolPeriodicEvaluationCriteriaPlaceholderAsync(docx, placeholder, criteriaContext);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(placeholder.PlaceholderType), $"Unsupported placeholder type: {placeholder.PlaceholderType}");
                }
            }
        }

        private async Task ProcessSchoolPeriodicEvaluationCriteriaPlaceholderAsync(DocX docx, PlaceholderDto placeholder, SchoolPeriodicEvaluationCriteriaContext? context)
        {
            if (context == null)
                throw new BusinessException("School periodic evaluation criteria context is required.");
            if (context.FormId == Guid.Empty || context.AcademicYearId == Guid.Empty || context.EvaluationRequestId == Guid.Empty)
                throw new BusinessException("School periodic evaluation criteria context contains an empty identifier.");

            var key = $"{{{{{NormalizePlaceholderKey(placeholder.Key)}}}}}";
            var paragraph = docx.Paragraphs.FirstOrDefault(p => p.Text.Contains(key, StringComparison.Ordinal));
            if (paragraph == null)
                throw new BusinessException($"The placeholder {key} was not found in the document.");
            if (!string.Equals(paragraph.Text.Trim(), key, StringComparison.Ordinal))
                throw new BusinessException($"The placeholder {key} must be alone in its paragraph.");

            var formResult = await serviceProvider.GetRequiredService<FormBL>()
                .GetFormItemsWithValues(context.FormId, context.AcademicYearId, context.EvaluationRequestId);
            if (formResult.IsFailed)
                throw new BusinessException($"Failed to retrieve school periodic evaluation criteria: {string.Join("; ", formResult.Errors.Select(e => e.Message))}");

            var font = GetTemplateFont(docx);
            var rootIndex = 0;
            foreach (var root in OrderedScopes(formResult.Value.Tree))
            {
                rootIndex++;
                InsertRootScope(paragraph, root, Segment(root.OrderNo, rootIndex), font);
            }

            paragraph.Remove(false);
        }

        private static string NormalizePlaceholderKey(string? key) => (key ?? string.Empty).Trim().Trim('{', '}');

        private static IEnumerable<ScopeTreeDto> OrderedScopes(IEnumerable<ScopeTreeDto>? scopes) =>
            (scopes ?? []).Select((x, i) => (Scope: x, Index: i + 1)).OrderBy(x => ValidOrder(x.Scope.OrderNo) ? x.Scope.OrderNo : x.Index).Select(x => x.Scope);

        private static IEnumerable<FormItemDto> OrderedItems(IEnumerable<FormItemDto>? items) =>
            (items ?? []).Select((x, i) => (Item: x, Index: i + 1)).OrderBy(x => ValidOrder(x.Item.OrderNo) ? x.Item.OrderNo : x.Index).Select(x => x.Item);

        private static bool ValidOrder(int orderNo) => orderNo > 0;
        private static string Segment(int orderNo, int index) => (ValidOrder(orderNo) ? orderNo : index).ToString();

        private void InsertRootScope(Paragraph anchor, ScopeTreeDto scope, string number, string font)
        {
            var ordinal = ArabicOrdinal(int.TryParse(number, out var n) ? n : 1);
            AddBefore(anchor, $"المعيار  {ordinal}: {scope.Name}", font, 15, true, Alignment.center, keepNext: true);
            AddBefore(anchor, $"مستوى {scope.Name} \" {GetJudgement(GetScopeAverage(scope))} \"", font, 12, true, Alignment.center, keepNext: true);

            var itemIndex = 0;
            foreach (var item in OrderedItems(scope.Items))
                InsertItem(anchor, item, $"{number}.{Segment(item.OrderNo, ++itemIndex)}", font);

            var childIndex = 0;
            foreach (var child in OrderedScopes(scope.Children))
                InsertScopeWithNumber(anchor, child, $"{number}.{Segment(child.OrderNo, ++childIndex)}", font, 1);
        }

        private void InsertScopeWithNumber(Paragraph anchor, ScopeTreeDto scope, string number, string font, int depth)
        {
            if (depth == 1)
            {
                var p = AddBefore(anchor, $"{number} {scope.Name}", font, 11, true, Alignment.center, keepNext: true);
                ApplyShading(p, ParseColor(scope.ColorCode) ?? Color.FromArgb(128, 0, 64));
            }
            else AddBefore(anchor, $"{number} {scope.Name}", font, 11, true, Alignment.right, keepNext: true);
            var itemIndex = 0;
            foreach (var item in OrderedItems(scope.Items)) InsertItem(anchor, item, $"{number}.{Segment(item.OrderNo, ++itemIndex)}", font);
            var childIndex = 0;
            foreach (var child in OrderedScopes(scope.Children)) InsertScopeWithNumber(anchor, child, $"{number}.{Segment(child.OrderNo, ++childIndex)}", font, depth + 1);
        }

        private void InsertItem(Paragraph anchor, FormItemDto item, string number, string font)
        {
            AddBefore(anchor, $"{number} {item.Name}", font, 10.5, true, Alignment.right, keepNext: true);
            if (!string.IsNullOrWhiteSpace(item.Note))
                AddBefore(anchor, $"‹ {item.Note}", font, 10, false, Alignment.both);
            foreach (var related in item.RelatedItems ?? [])
                if (!string.IsNullOrWhiteSpace(related.Note))
                    AddBefore(anchor, $"‹ {related.Note}", font, 10, false, Alignment.both);
        }

        private Paragraph AddBefore(Paragraph anchor, string text, string font, double size, bool bold, Alignment alignment, bool keepNext = false)
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
            void Add(string name) { if (pPr.Element(w + name) == null) pPr.Add(new XElement(w + name)); }
            Add("bidi"); Add("keepLines"); Add("widowControl"); if (keepNext) Add("keepNext");
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

        private static Color? ParseColor(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            try { return ColorTranslator.FromHtml(value.StartsWith('#') ? value : "#" + value); }
            catch { return null; }
        }
        private static string ArabicOrdinal(int n) => n switch { 1 => "الأول", 2 => "الثاني", 3 => "الثالث", 4 => "الرابع", 5 => "الخامس", 6 => "السادس",7 => "السابع",8 => "الثامن",9 => "التاسع",10 => "العاشر", _ => n.ToString() };
        private static string GetTemplateFont(DocX docx) => "Arial";
        private decimal GetScopeAverage(ScopeTreeDto scope) { var items = GetAllItems(scope); return items.Where(x => x.Value.HasValue).Select(x => x.Value!.Value).DefaultIfEmpty(0).Average(); }
        private static List<FormItemDto> GetAllItems(ScopeTreeDto scope) { var result = new List<FormItemDto>(); result.AddRange(scope.Items ?? []); foreach (var child in scope.Children ?? []) result.AddRange(GetAllItems(child)); return result; }
        private static string GetJudgement(decimal value) => value switch { < 3m => "ضعيف", < 3.75m => "مقبول", < 4.25m => "جيد", < 4.75m => "جيد جداً", _ => "ممتاز" };

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
            if(table.Rows.Count < 2) return; // Ensure there is at least one template row
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
            
            foreach (var match in from cell in row.Cells from para in cell.Paragraphs select Regex.Matches(para.Text, @"{{(.*?)}}") into matches from Match match in matches select match)
            {
                placeholderKeys.Add(match.Value);
            }
            
            return placeholderKeys;
        }

        private List<PlaceholderDto> GetRelevantPlaceholders(List<PlaceholderDto> allPlaceholders, HashSet<string> placeholderKeys)
        {
            return allPlaceholders
                .Where(p =>!string.IsNullOrEmpty(p.Key) && placeholderKeys.Contains(p.Key))
                .ToList();
        }

        private Dictionary<string, RowChildData> PreparePlaceholderData(List<PlaceholderDto> relevantPlaceholders)
        {
            var placeholderData = new Dictionary<string, RowChildData>();
            
            foreach (var placeholder in relevantPlaceholders
                         .Where(x=> !string.IsNullOrEmpty(x.Key) && !string.IsNullOrEmpty(x.ChildFieldId)))
            {
                var rows = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(placeholder.Value ?? "[]") ?? [];
                placeholderData[placeholder.Key!] = new RowChildData(Guid.Parse(placeholder.ChildFieldId!), placeholder.ChildField, rows);
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

}