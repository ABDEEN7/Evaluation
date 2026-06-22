using Evaluation.DAL.Models.Template;
using Evaluation.DAL.Repositories;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.Services.Special;

public class EmailNotificationService(CacheDataProvider cacheDataProvider,
    EmailTemplateProvider emailTemplateProvider, IEmailServices emailServices, UnitOfWork unitOfWork) : IEmailNotificationService
{

    public async Task<bool> SendByTemplateAsync(
        string templateSetting,
        List<string> emails)
    {
        var templateKey = await cacheDataProvider
        .GetSystemSettingValue(templateSetting);

        var config = await emailTemplateProvider
            .BuildEmailMessageModelConfig(templateKey);

        config.messageModel.ToEmails = emails;

        var template = await unitOfWork.GetRepository<EmailTemplate>()
            .GetAllNonDeleted(x => x.BackendName == templateKey)
            .FirstOrDefaultAsync();

        if (template == null)
            throw new BusinessException(ConstantKeys.ExceptionMessage.TemplateNotFound);

        config.messageModel.Subject = template.TemplateSubject;
        config.messageModel.Body = template.TemplateBody;
        config.messageModel.ModuleBackendName = ConstantKeys.Module.Alert;

        return await emailServices.SendEmail(config.messageModel);
    }
}