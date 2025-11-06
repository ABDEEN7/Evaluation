using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scholarship.DAL.Framework;
using Scholarship.DAL.Models.ActionEntities;
using Scholarship.DAL.Models.Attachments;
using Scholarship.DAL.Models.Base;
using Scholarship.DAL.Models.PartyTypeEntities;
using Scholarship.DAL.Models.ScholarshipEntity;
using Scholarship.DAL.Models.ServiceRequestEntities;
using Scholarship.DAL.Models.Templates;
using Scholarship.Services.BusinessLayer.API.Template;
using Scholarship.Services.Extensions;
using Scholarship.Services.Models;
using Scholarship.Services.Models.API;
using Scholarship.Services.Models.SMTP;
using Scholarship.Services.Special;
using Scholarship.SharedHelper.Enums;
using Scholarship.SharedHelper.Exceptions;
using Scholarship.SharedHelper.Models;
using Spire.Doc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Evaluation.Services.BusinessLayer.API.FormBuilderLayer.Srvices
{
    public class SrvNotification(IServiceScopeFactory serviceScopeFactory, IEmailServices emailServices, CacheDataProvider cacheDataProvider, MasterBL masterBL, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceProvider serviceProvider, RequestInfo _requestInfo, EmailTemplateProvider emailTemplateProvider)
            : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, _requestInfo)
        {
        public async Task HandleNotification(IList<ActionStatusConfigNotification> notifications, ServiceRequest request, Guid actionId, string lang, string remarks, List<Attachment>? actionOtherAttachments = null)
        {

            if (notifications != null)
            {
                using var scopedUow = serviceScopeFactory.CreateScopedUow();
                
                var userProfileRepo = scopedUow.GetRepository<UserProfile>();
                var partyTypeRepo = scopedUow.GetRepository<PartyType>();
                var requestRepo = scopedUow.GetRepository<ServiceRequest>();

                var allUserProfiles =await userProfileRepo.GetAllQueryFiltered()
                    .Include(u => u.UserPartTypes)
                    .ToListAsync();

                var requestData =await requestRepo.GetAllQueryFiltered()
                    .Include(r => r.ServiceRequestFieldsValues)
                    .Include(r => r.Service)
                    .FirstOrDefaultAsync(r => r.Id == request.Id);


                if (requestData == null)
                    return;

                foreach (var notification in notifications)
                {
                    var partyType = await partyTypeRepo.GetByIDActiveNonDeleted(notification.PartyTypeId);
                    if (partyType == null) continue;

                  
                     var recipients = partyType.IsEmployeePartyType
                        ? GetEmployeeRecipients( request.Id, request.CountryId,request.UniversityId, allUserProfiles, partyType)
                        : GetStudentRecipient(request.StudentId, allUserProfiles);

                    if (!recipients.Any()) continue;

                    if (notification.IsEmailSend && notification.EmailTemplateId.HasValue)
                    {
                        await sendEmail(recipients, notification.EmailTemplateId.Value, requestData,
                                        actionId, partyType.Id, lang, remarks, actionOtherAttachments!);
                    }

                    if (notification.IsMessageSend && notification.SMSTemplateId.HasValue)
                    {
                        await SendSms(recipients, notification.SMSTemplateId.Value, requestData, actionId);
                    }

                    if (notification.IsNotificationSend && notification.NotificationTemplateId.HasValue)
                    {
                        await SendNotification(recipients, notification.NotificationTemplateId.Value,
                                               requestData, actionId, lang, partyType.Id);
                    }
                }

                await scopedUow.CommitAsync();
            }
        }
        private List<UserProfile> GetEmployeeRecipients( Guid serviceRequestId, Guid? requestCountryId, Guid? requestUniversityId, List<UserProfile> allUserProfiles, PartyType partyType)
        {
            var assignmentRepo = serviceScopeFactory.CreateScopedUow().GetRepository<RequestAssignment>();

            var assignments = assignmentRepo.GetAllQueryFiltered()
                .Where(a => a.ServiceRequestId == serviceRequestId)
                .Include(a => a.MinistryUser.UserPartTypes)
                .ToList();

            // 1. Directly assigned users in this PartyType
            var assignedUsers = assignments
                .Where(a => a.MinistryUser.UserPartTypes!.Any(pt => pt.PartyTypeId == partyType.Id))
                .Select(a => a.MinistryUser)
                .ToList();

            var usersWithViewAllOrCountryAccess = allUserProfiles
                     .Where(u => u.UserPartTypes?.Any(pt =>
                         pt.PartyTypeId == partyType.Id && (
                             // Case 1: Can view all requests, no country/university restriction
                             (pt.PartyType!.CanViewAllRequests &&
                              (pt.PartyType.PartyTypeCountyUniversity == null ||
                               pt.PartyType.PartyTypeCountyUniversity.Count == 0))
                             ||
                             // Case 2: Restricted, but matches country/university
                             pt.PartyType!.PartyTypeCountyUniversity!.Any(ctu =>
                                 ctu.CountryId == requestCountryId &&
                                 (ctu.UniversityId == null || ctu.UniversityId == requestUniversityId))
                         )) == true)
                     .ToList();

            // Merge: assigned + viewAll + country/univ access
            assignedUsers.AddRange(usersWithViewAllOrCountryAccess);

            // Remove duplicates
            return assignedUsers
                .GroupBy(u => u.Id)
                .Select(g => g.First())
                .ToList();
        }
        private List<UserProfile> GetStudentRecipient(Guid? studentId, List<UserProfile> allUserProfiles)
        {
            if (studentId == null) return new List<UserProfile>();

            var student = allUserProfiles.FirstOrDefault(u => u.Id == studentId.Value);
            return student != null ? new List<UserProfile> { student } : new List<UserProfile>();
        }
        private async Task sendEmail( List<UserProfile> recipients,Guid emailTemplateId,ServiceRequest requestData,  Guid actionId, Guid? partyTypeId = null, string lang = "ar",string remarks = null!, List<Attachment> actionOtherAttachments = null!)
        {
            using var scopedUow = serviceScopeFactory.CreateScopedUow();

            var emailTemplate = await scopedUow.GetRepository<EmailTemplate>()
                .GetAllQueryFiltered(x => x.Id == emailTemplateId)
                .Include(x => x.EmailProfile)
                .FirstOrDefaultAsync();

            if (emailTemplate == null)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.EmailtemplateNotFound  );
            }

            var templateEngine = serviceProvider.GetRequiredService<TemplateBl>();

            var emailBody = await templateEngine.GetRequestEmailTemplate(requestData, emailTemplate.TemplateBody, partyTypeId, lang, remarks);
            var emailSubject = await templateEngine.GetRequestEmailTemplate(requestData, emailTemplate.TemplateSubject, partyTypeId, lang, remarks);
            var generatedAttachments = await templateEngine.GetRequestEmailAttachment(requestData, emailTemplateId, lang);

            var emailModel = await emailTemplateProvider.BuildEmailMessageModelConfig(emailTemplate.BackendName);
            emailModel.messageModel.ToEmails = recipients.Select(r => r.Email).Where(e => !string.IsNullOrWhiteSpace(e)).Distinct().ToList();
            emailModel.messageModel.Subject = emailSubject;
            emailModel.messageModel.Body = emailBody;
            emailModel.messageModel.RefId = requestData.Id;
            emailModel.messageModel.ModuleBackendName = ConstantKeys.Module.Alert;
            emailModel.messageModel.AttachmentArrays = generatedAttachments;
            emailModel.messageModel.Attachments = new();

               // Attach from Action or Request Fields
               await AddDynamicAttachments(emailTemplate, requestData, emailModel.messageModel, actionOtherAttachments, scopedUow);

            await emailServices.SendEmail(emailModel.messageModel);
        }
        private async Task AddDynamicAttachments(EmailTemplate template, ServiceRequest requestData, EmailMessageModel emailModel, List<Attachment> actionOtherAttachments, UnitOfWork uow)
        {
            var templateEngine = serviceProvider.GetRequiredService<TemplateBl>();

            if (!string.IsNullOrWhiteSpace(template.fielsFromRequest))
            {
                foreach (var field in template.fielsFromRequest.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    if (field.Trim() == "ActionOtherAttachement" && actionOtherAttachments != null)
                    {
                        foreach (var att in actionOtherAttachments)
                        {
                            if (att?.Id != null)
                                emailModel.Attachments.Add(await templateEngine.HandleAttachment(att.Id));
                        }
                    }
                    else if (Guid.TryParse(field.Trim(), out var fieldGuid))
                    {
                        var fieldValue = await uow.GetRepository<ServiceRequestFieldsValue>()
                            .GetAllQueryFiltered(f => f.ServiceRequestId == requestData.Id && f.FieldId == fieldGuid)
                            .FirstOrDefaultAsync();

                        if (!string.IsNullOrWhiteSpace(fieldValue?.Value) && Guid.TryParse(fieldValue.Value, out var fileId))
                            emailModel.Attachments.Add(await templateEngine.HandleAttachment(fileId));
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(template.fielsFromScholarship) && requestData.ScholarshipId.HasValue)
            {
                foreach (var schField in template.fielsFromScholarship.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    if (Guid.TryParse(schField.Trim(), out var fieldId))
                    {
                        var value = await uow.GetRepository<SchFieldValue>()
                            .GetAllQueryFiltered()
                            .Where(f => f.ScholarshipId == requestData.ScholarshipId && f.SystemFieldId == fieldId)
                            .FirstOrDefaultAsync();

                        if (!string.IsNullOrWhiteSpace(value?.Value) && Guid.TryParse(value.Value, out var fileId))
                            emailModel.Attachments.Add(await templateEngine.HandleAttachment(fileId));
                    }
                }
            }
        }
        private async Task SendSms(List<UserProfile> recipients, Guid smsTemplateId, ServiceRequest requestData, Guid actionId)
        {
            var lang = _requestInfo?.Lang ?? "ar";

            using var scopedUow = serviceScopeFactory.CreateScopedUow();
            var template = await scopedUow.GetRepository<SMSTemplate>().GetByIDActiveNonDeleted(smsTemplateId);

            if (template == null)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.SMSTemplateNotFound);
            }

            var smsProfiles = await cacheDataProvider.GetSMSProfiles();

            if (!smsProfiles.Any())
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.SMSProfileNotFound );
            }
            var smsProfile = smsProfiles.Where(x => x.Id == template.SMSProfileId).FirstOrDefault();
            if (smsProfile == null)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.SMSProfileNotFound  );
            }


           
            var engine = serviceProvider.GetRequiredService<TemplateBl>();
            var message = await engine.GetRequestEmailTemplate(requestData, template.Messages, null, lang);

            var smsService = masterBL.SmsServices;
            var tasks = recipients.OfType<StudentUser>()
                .Where(u => !string.IsNullOrEmpty(u.Mobile))
                .Select(u => smsService.SendMessage(new SMSMessageModel
                {
                    BaseUrl = smsProfile.BaseUrl,
                    UserName = smsProfile.UserName,
                    Password = smsProfile.Password,
                    mobile = u.Mobile!,
                    message = message,
                    RefId = requestData.Id
                })).ToList();

            await Task.WhenAll(tasks);
        }
        private async Task SendNotification(List<UserProfile> recipients, Guid notificationTemplateId, ServiceRequest requestData, Guid actionId, string lang, Guid? partyTypeId = null)
        {
            using var scopedUow = serviceScopeFactory.CreateScopedUow();
            var template = await scopedUow.GetRepository<NotificationTemplate>().GetByIDActiveNonDeleted(notificationTemplateId);

            if (template == null)
            {
                throw new BusinessException($"Notification template with ID {notificationTemplateId} not found.");
            }

            var engine = serviceProvider.GetRequiredService<TemplateBl>();
            var bodyAr = await engine.GetRequestEmailTemplate(requestData, template.TemplateBodyAr, partyTypeId, lang);
            var bodyEn = await engine.GetRequestEmailTemplate(requestData, template.TemplateBodyEn, partyTypeId, lang);
            var titleAr = await engine.GetRequestEmailTemplate(requestData, template.TemplateSubjectAr!, partyTypeId, lang);
            var titleEn = await engine.GetRequestEmailTemplate(requestData, template.TemplateSubjectEn!, partyTypeId, lang);

            var notifications = recipients.Select(user => new Notification
            {
                TitleAr = titleAr,
                TitleEn = titleEn,
                DescriptionAr = bodyAr,
                DescriptionEn = bodyEn,
                NotificationTemplateId = notificationTemplateId,
                ReadCount = 0,
                UserProfileId = user.Id
            }).ToList();

            await scopedUow.GetRepository<Notification>().InsertRange(notifications);
            await scopedUow.CommitAsync();
        }

    }

}
