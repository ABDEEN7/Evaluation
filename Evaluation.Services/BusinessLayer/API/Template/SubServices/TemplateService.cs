using System.Net.Mail;
using System.Text.RegularExpressions;
using Evaluation.DAL.Entities.Website;
using Evaluation.DAL.UnitOfWork;
using Evaluation.Services.Special;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Evaluation.Services.BusinessLayer.API.Template;
using Evaluation.DAL.Entities.FormBuilder;
using Xceed.Document.NET;
using Xceed.Words.NET;

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

        public async Task<byte[]> GenerateAttachments(Guid templateId, List<PlaceholderDto> placeholders, Guid systemModuleId)
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
        
        public async Task<byte[]> HandleAttachment(List<PlaceholderDto> placeholders, Guid attachmentId, Guid systemModuleId)
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
                        placeholder.Key = placeholder.Key?.Trim('{', '}'); // Remove curly braces
                        docx.ReplaceText(new StringReplaceTextOptions
                        {
                            SearchValue = $"{{{{{placeholder.Key}}}}}",
                            NewValue = placeholder.Value
                        });
                        break;

                case PlaceholderType.Image:
                    placeholder.Key = placeholder.Key?.Trim('{', '}');

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
                                SearchValue = $"{{{{{placeholder.Key}}}}}",
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
                        placeholder.Key = placeholder.Key?.Trim('{', '}'); // Remove curly braces
                        if (!string.IsNullOrWhiteSpace(placeholder.Value))
                        {
                            var data = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(placeholder.Value) ?? [];
                            var childrenFieldIds = placeholders.FirstOrDefault(p => p.Key == placeholder.Key)?
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
                                        field.DropDownTypeId, $"{value}", requestInfo.Lang,placeholder.Key);
                                    docxTable.Rows[rowIndex + 1].Cells[colIndex].Paragraphs.First().Append(finalValue);
                                    colIndex++;
                                }
                            }
                            docxTable.Design = TableDesign.LightShading;
                            docxTable.AutoFit = AutoFit.Contents;
                            docx.ReplaceTextWithObject(new ObjectReplaceTextOptions
                            {
                                SearchValue = $"{{{{{placeholder.Key}}}}}",
                                NewObject = docxTable,
                                TrackChanges = false
                            });
                        }
                        break;
                    case PlaceholderType.Table:
                        var tablePlaceholders = placeholders
                            .Where(x => x.PlaceholderType == PlaceholderType.Table)
                            .ToList();
                        ProcessTablePlaceholdersAsync(docx, tablePlaceholders);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(placeholder.PlaceholderType), $"Unsupported placeholder type: {placeholder.PlaceholderType}");
                }
            }
        }
        private void ProcessTablePlaceholdersAsync(DocX docx, List<PlaceholderDto> tablePlaceholders)
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