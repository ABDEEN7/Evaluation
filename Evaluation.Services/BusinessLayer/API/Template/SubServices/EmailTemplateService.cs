using Evaluation.DAL.Entities.Attachments;
using Evaluation.DAL.Entities.ServicesEntities;
using Evaluation.Services.Special;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Microsoft.EntityFrameworkCore;


namespace Evaluation.Services.BusinessLayer.API.Template
{
    public class EmailTemplateService(
        TemplateService templateService,
        PlaceholderService placeholderService,
        IServiceProvider serviceProvider,
        AzureBlobStorageService blobService)
        : ApiServiceBase
    {
        public async Task<string> GetRequestEmailTemplate(ServiceRequest request, string template, Guid? partyTypeId = null, string lang = "ar", string? remarks = null)
        {
            if (string.IsNullOrEmpty(template)) return string.Empty;

            var scopedUow = serviceProvider.CreateScopedUow();
            var placeholders = scopedUow.GetRepository<PlaceHolder>()
                .GetAllQueryFiltered().Where(c => c.ServiceId == request.ServiceId).ToList();

            var result = new List<PlaceholderDto>();
            result.AddRange(await placeholderService.GetRequestFieldPlaceHolders(scopedUow, 
                placeholders.Where(p => p.TypeDisplay == ConstantKeys.PlaceHolderTypes.RequestField).ToList(), 
                request, lang));
            result.AddRange(await placeholderService.GetScholarshipFieldPlaceHolders(scopedUow, 
                placeholders.Where(p => p.TypeDisplay == ConstantKeys.PlaceHolderTypes.ScholarshipField).ToList(), 
                request, lang));

            return templateService.GetTextFromHtml(template, result);
        }

        public async Task<List<byte[]>> GetRequestEmailAttachment(ServiceRequest requestData, Guid emailTemplateId, string lang)
        {
            var result = new List<byte[]>();
            var scopedUow = serviceProvider.CreateScopedUow();
            var requestPlaceholders = await placeholderService.GetRequestPlaceHolders(requestData.Id, lang);
            var service = await scopedUow.GetRepository<Service>()
                .GetAllQueryFiltered(s => s.Id == requestData.ServiceId)
                .FirstOrDefaultAsync() ?? throw new BusinessException(ConstantKeys.ExceptionMessage.ServiceRequestNotFound);

            var emailTemplateDocuments = scopedUow.GetRepository<EmailTemplateDocument>()
                .GetAllQueryFiltered()
                .Include(d => d.TemplateDoc.TemplateGenrationType)
                .Where(d => d.EmailTemplateId == emailTemplateId).ToList();

            foreach (var doc in emailTemplateDocuments)
            {
                requestPlaceholders.AddRange(await placeholderService.GetSystemModulePlaceHoldersByTemplateId(doc.TemplateDocId, lang));
                var generationType = doc.TemplateDoc.TemplateGenrationType!.BackendName;
                var templateId = doc.TemplateDocId;
                using var client = new HttpClient();
                
                switch (generationType)
                {
                    case "AttachmentWithoutPlaceHolder":
                        var attachment = await scopedUow.GetRepository<Attachment>().GetByIDActiveNonDeleted(templateId);
                        if (attachment == null) throw new BusinessException(ConstantKeys.ExceptionMessage.InActiveData);

                        var url = blobService.GenerateSasToken(attachment.FileName);
                        result.Add(await client.GetByteArrayAsync(url));
                        break;

                    default:
                        result.Add(await templateService.GenerateAttachments(templateId, requestPlaceholders, service.SystemModuleId));
                        break;
                }
            }

            return result;
        }
    }
}