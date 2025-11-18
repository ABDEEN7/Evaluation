using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Attachments;
using Evaluation.DAL.Models.FormBuilder;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api.AttachmentsDTOs;
using Evaluation.SharedHelper.Models.Api.FormBuilderDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System.Text.RegularExpressions;

namespace Evaluation.Services.BusinessLayer.API.FormBuilderLayer.Srvices
{
    public class  SrvAttachments( AzureBlobStorageService StorageService, IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceProvider serviceProvider, RequestInfo requestInfo)
            : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
        {
           
        public async Task<string?> GetAttachmentById(Guid attachmentId, Guid requestId)
        {
            var attachment = await serviceScopeFactory.CreateScopedUow()
                                    .GetRepository<Attachment>()
                                    .GetAllQueryFiltered(x => x.Id == attachmentId ).FirstOrDefaultAsync();
            if (null == attachment)
            {
                return null;
            }
            else
                return StorageService.GenerateSasToken(attachment.FileName, uiFileName: attachment.UiFileName);

        } 
        public async Task<Attachment?> GetRequestAttachmentsByfieldId(Guid fieldId, Guid requestId)
        {
            var attachment = await serviceScopeFactory.CreateScopedUow()
                                    .GetRepository<Attachment>()
                                    .GetAllQueryFiltered(x => x.FieldId == fieldId && x.ServiceRequestId == requestId).FirstOrDefaultAsync();
            
                return attachment;

        }
        public async Task<List<FieldValueDTO>> UploadAndInsertAttachments(List<FieldValueDTO> FieldValueDTOs, Guid? RequestId, Guid? EvaluationRequestId, List<FileFieldDTO> fileFields, List<IFormFile> files)
        {
            if (files.Any())
            {
                var uploadedFiles = await StorageService.UploadFormFilesAsync(fileFields);

                if (uploadedFiles.Any())
                {
                    if (uploadedFiles.Count() != files.Count())
                    {
                        throw new BusinessException(ConstantKeys.ExceptionMessage.IncompleteRequest);
                    }

                    var attachmentsToBeInserted = new List<Attachment>();
                    uploadedFiles.ForEach(fileDTO =>
                    {

                        var field = FieldValueDTOs.FirstOrDefault(c => c.FieldId == fileDTO.FieldId);
                        string[] parts = fileDTO.FileName.Contains("_") ? fileDTO.FileName.Split('_') : new string[] { fileDTO.FileName };

                        if (field != null)
                        {
                            var attachment = new Attachment
                            {
                                FieldId = field.FieldId,
                                FileName = fileDTO.CustomFileName,
                                UiFileName = parts.Length > 2 ? parts[2] : fileDTO.FileName,
                                FileExtension = Path.GetExtension(fileDTO.FileName),
                                FileSize = fileDTO.FileLength!.Value,
                                ServiceRequestId = RequestId,
                                ChildFieldId = parts.Length > 2 ? Guid.Parse(parts[0]) : null,
                                Index = parts.Length > 2 ? parts[1] : null,
                                EvaluationRequestId= EvaluationRequestId,
                            };

                            attachmentsToBeInserted.Add(attachment);
                        }
                    });

                    var result = await InsertAttachments(attachmentsToBeInserted);

                    foreach (var attachment in result)
                    {
                        var Field = await GetFieldsByIdsAsync( attachment.FieldId!.Value );
                        var fileField = FieldValueDTOs.FirstOrDefault(c => c.FieldId == attachment.FieldId);

                        if (fileField != null)
                        {
                            if (Field!.FieldType!.BackendName == ConstantKeys.FieldTypeConstant.file || Field.FieldType.BackendName == ConstantKeys.FieldTypeConstant.fileV2)
                            {
                                fileField.Value = attachment.Id.ToString();
                            }
                            else if (Field.FieldType.BackendName == ConstantKeys.FieldTypeConstant.list)
                            {
                                var jsonList = JsonConvert.DeserializeObject<List<JObject>>(fileField.Value!.ToString());
                                foreach (var item in jsonList!)
                                {
                                    if (item.ContainsKey("Index") && item["Index"]?.ToString() == attachment.Index)
                                    {
                                        var key = Convert.ToString(attachment.ChildFieldId);
                                        if (item.ContainsKey(attachment.ChildFieldId?.ToString()!))
                                        {
                                            item[key!] = attachment.Id.ToString();
                                        }
                                    }
                                }

                                fileField.Value = JsonConvert.SerializeObject(jsonList);
                            }
                        }
                    }
                }
            }
            return FieldValueDTOs;
        }
        public async Task<List<Attachment>> UploadAndInsertOtherAttachments(List<IFormFile> files, Guid? actionlog,Guid? EvaluationRequestId)
        {
            if (files.Any())
            {

               await ValidateAttachmentFilesAsync(files);
                var uploadedFiles = await StorageService.UploadFormFilesAsync(files);
                if (uploadedFiles.Any())
                {
                    if (uploadedFiles.Count() != files.Count())
                    {
                        throw new BusinessException(ConstantKeys.ExceptionMessage.IncompleteRequest);
                    }
                    var attachmentsToBeInserted = new List<Attachment>();
                    uploadedFiles.ForEach(fileDTO =>
                    {
                        var attachment = new Attachment()
                        {
                            ActionTransactionsLogId = actionlog,
                            FileName = fileDTO.CustomFileName,
                            UiFileName = fileDTO.FileName,
                            FileExtension = Path.GetExtension(fileDTO.FileName),
                            FileSize = fileDTO.FileLength!.Value,
                            IsOthers = true,
                            EvaluationRequestId= EvaluationRequestId,

                        };

                        attachmentsToBeInserted.Add(attachment);
                    });

                    var result = await InsertAttachments(attachmentsToBeInserted);
                    return result.ToList();

                }
                return null!;

            }
            return null!;

        }
        public async Task<IEnumerable<Attachment>> InsertAttachments(List<Attachment> attachments)
        {
            return await uow.GetRepository<Attachment>().InsertRange(attachments);
        }
        public Guid? GetFieldIdFromFile(string contentDisposition, int index)
        {
            Match match = Regex.Match(contentDisposition, @"Files\[(.*?)\]");

            if (match.Success && match.Groups.Count > index)
            {
                if (Guid.TryParse(match.Groups[index].Value, out Guid fieldId))
                {
                    return fieldId;
                }
            }

            return null;
        }
        public (List<IFormFile> filesWithFieldId, List<IFormFile> othersAttachments) SeparateFilesByFieldId(IFormFileCollection files)
        {
            var filesWithFieldId = new List<IFormFile>();
            var othersAttachments = new List<IFormFile>();

            foreach (var file in files)
            {
                if (file != null && file.Length > 0)
                {
                    var fieldId = GetFieldIdFromFile(file.ContentDisposition, 1);
                    if (fieldId != null)
                    {
                        filesWithFieldId.Add(file);
                    }
                    else
                    {
                        othersAttachments.Add(file);
                    }
                }
            }

            return (filesWithFieldId, othersAttachments);
        }
        public async Task<Field?> GetFieldsByIdsAsync(Guid fieldId)
        {
            var fields = await serviceScopeFactory.CreateScopedUow()
                .GetRepository<Field>()
                .GetAllActiveNonDeleted(x => x.Id == fieldId)
                .Include(x => x.FieldType)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return fields;
        }
        public async Task<string?> UploadIntegrationFileAsync(  Guid? parentFieldId, Guid? childFieldId,  Guid? index,  JObject certificateJToken,Guid? ServiceRequestId)
        {
            // Extract values from JObject
            var base64Token = certificateJToken.SelectToken("DocumentBase64", errorWhenNoMatch: false);
            var fileNameToken = certificateJToken.SelectToken("DocumentPath", errorWhenNoMatch: false);
            var contentTypeToken = certificateJToken.SelectToken("ContentType", errorWhenNoMatch: false); // Optional

            if (base64Token == null || fileNameToken == null)
                return null;

            byte[] fileBytes;
            try
            {
                fileBytes = Convert.FromBase64String(base64Token.ToString());
            }
            catch
            {
                return null; // Invalid base64
            }

            var fileName = Path.GetFileName(fileNameToken.ToString());
            var contentType = contentTypeToken?.ToString() ?? "application/octet-stream";

            var fileFieldDto = new FileFieldDTO
            {
                FieldId = parentFieldId,
                childFieldId = childFieldId,
                Index = index,
                FileName = fileName,
                ContentType = contentType,
                FileBytes = fileBytes
            };

            var uploadedFiles = await StorageService.UploadFormFilesAsync(new List<FileFieldDTO> { fileFieldDto });
            var uploadedFile = uploadedFiles.FirstOrDefault();

            if (uploadedFile == null)
                return null;

            var attachment = new Attachment
            {
                FieldId = parentFieldId,
                ChildFieldId = childFieldId,
                Index = index?.ToString(),
                FileName = uploadedFile.CustomFileName,
                UiFileName = fileName,
                FileExtension = Path.GetExtension(fileName),
                FileSize = fileBytes.Length,
                ServiceRequestId = ServiceRequestId
            };

            var inserted = await InsertAttachments(new List<Attachment> { attachment });
            var savedAttachment = inserted.FirstOrDefault();

            return savedAttachment?.Id.ToString();
        }
        public async Task<List<AttachementDTO?>> GetAttachmentsByIdsAsync(List<string> attachmentIds)
        {
            if (attachmentIds == null || !attachmentIds.Any())
                return new List<AttachementDTO?>();

            var attachmentGuids = attachmentIds
                .Where(id => Guid.TryParse(id, out _))
                .Select(Guid.Parse)
                .ToList();

            var attachments = await serviceScopeFactory.CreateScopedUow()
                .GetRepository<Attachment>()
                .GetAllQueryFiltered()
                .Where(c => attachmentGuids.Contains(c.Id))
                .Select(m => new AttachementDTO
                {
                    Id = m.Id,
                    UiFileName = m.UiFileName
                }).ToListAsync();

            return attachments!;
        }
        public async Task<byte[]> GetBytesByAttachmentIdAsync(string attachmentId)
        {
            if (!Guid.TryParse(attachmentId, out var id))
                throw new BusinessException("Invalid attachment id.");

            var repo = serviceScopeFactory.CreateScopedUow().GetRepository<Attachment>();
            var attachment = await repo.GetAllQueryFiltered(x => x.Id == id).FirstOrDefaultAsync();

            if (attachment == null)
                throw new BusinessException($"Attachment {attachmentId} not found.");                       

            // Use AzureBlobStorageService to download raw bytes
            var bytes = await StorageService.DownloadFileBytesAsync(attachment.FileName);                   

            return bytes;
        }
        public async Task<IReadOnlyList<Attachment>> UpdateRequestAttachmentsAsync(Guid requestId,Guid EvaluationRequestId)
        {
            using var scopedUow = serviceScopeFactory.CreateScopedUow();
            var repo = scopedUow.GetRepository<Attachment>();

            var attachments = await repo
                .GetAllQueryFiltered(a => a.ServiceRequestId == requestId)
                .ToListAsync();

            if (attachments.Count == 0)
                return attachments; 

            foreach (var a in attachments)
            {
                a.EvaluationRequestId = EvaluationRequestId;
            }
            repo.UpdateRange(attachments);
            return attachments;
        }
        public async Task ValidateAttachmentFilesAsync(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
                throw new BusinessException(ConstantKeys.ExceptionMessage.Attachment_NoFiles);

            var allowedExtensionsSetting = await cacheDataProvider.GetSystemSettingValue(ConstantKeys.SystemSettings.AddAttachment_FILE_EXTENSION);
            var allowedExtensions = allowedExtensionsSetting?
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim().ToLower())
                .ToList() ?? new List<string> { ".pdf", ".jpg", ".jpeg", ".png" };

            var maxSizeStr = await cacheDataProvider.GetSystemSettingValue(ConstantKeys.SystemSettings.AddAttachment_FILE_SIZE);
            var maxFileSizeMB = int.TryParse(maxSizeStr, out var sizeVal) ? sizeVal : 10;

            var maxCountStr = await cacheDataProvider.GetSystemSettingValue(ConstantKeys.SystemSettings.AddAttachment_FILE_COUNT);
            var maxFileCount = int.TryParse(maxCountStr, out var countVal) ? countVal : 10;

            if (files.Count > maxFileCount)
                throw new BusinessException(ConstantKeys.ExceptionMessage.Attachment_TooManyFiles);

            foreach (var file in files)
            {
                var ext = Path.GetExtension(file.FileName)?.ToLower();
                if (string.IsNullOrWhiteSpace(ext) || !allowedExtensions.Contains(ext))
                    throw new BusinessException($"{ConstantKeys.ExceptionMessage.Attachment_InvalidExtension} ({file.FileName})");

                var maxBytes = maxFileSizeMB * 1024 * 1024;
                if (file.Length > maxBytes)
                    throw new BusinessException($"{ConstantKeys.ExceptionMessage.Attachment_TooLarge} ({file.FileName})");
            }
        }


    }


}
