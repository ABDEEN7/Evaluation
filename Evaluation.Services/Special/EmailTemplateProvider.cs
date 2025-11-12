using Evaluation.Services.Models.SMTP;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models.Api.TemplatesDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.Services.Special
{
	public class EmailTemplateProvider
	{

		#region Init
		private readonly IEmailServices emailServices;
		private readonly CacheDataProvider cacheDataProvider;
		private readonly LoggingServices loggingServices;

		public EmailTemplateProvider(IEmailServices emailServices,
			CacheDataProvider cacheDataProvider, LoggingServices loggingServices)
		{
			this.emailServices = emailServices;
			this.cacheDataProvider = cacheDataProvider;
			this.loggingServices = loggingServices;
		}


		public async Task<(EmailMessageModel messageModel, EmailTemplateDTO emailTemplate)> BuildEmailMessageModelConfig(string backendname)
		{
			if (string.IsNullOrEmpty(backendname))
			{
				throw new BusinessException($"you must provide the email template backendname");
			}

			var emailProfiles = await cacheDataProvider.GetEmailProfiles();

			if (!emailProfiles.Any())
			{
				throw new BusinessException($"no email profile found");
			}

			var emailTemplates = await cacheDataProvider.GetEmailTemplates();

			if (!emailTemplates.Any())
			{
				throw new BusinessException($"no email template found");
			}

			var emailTemplate = emailTemplates.Where(x => x.BackendName == backendname).FirstOrDefault();
			if (emailTemplate == null)
			{
				throw new BusinessException($"no email template found");
			}

			var emailProfile = emailProfiles.Where(x => x.Id == emailTemplate.EmailProfileId).FirstOrDefault();
			if (emailProfile == null)
			{
				throw new BusinessException($"email profile not found");
			}

			var emailMessageModel = new EmailMessageModel
			{
				SenderAddress = emailProfile.SenderAddress,
				SenderDisplayName = emailProfile.SenderDisplayName,
				UserName = emailProfile.UserName,
				Password = emailProfile.Password,
				Host = emailProfile.Host,
				Port = emailProfile.Port,
				EnableSSL = emailProfile.EnableSSL,
				UseDefaultCredentials = emailProfile.UseDefaultCredentials,
				IsBodyHTML = emailProfile.IsBodyHTML,
				EmailRequestTimeout = emailProfile.EmailRequestTimeout,

			};

			return (emailMessageModel, emailTemplate);

		}

		#endregion
		public async Task<bool> SendTestEmail(string email)
		{

			if (string.IsNullOrEmpty(email))
			{
				throw new BusinessException($"you must provide the email");
			}

			var config = await BuildEmailMessageModelConfig(ConstantKeys.EmailTemplateList.TestEmail);
			config.messageModel.ToEmails = new List<string> { email };

			if (!string.IsNullOrEmpty(config.emailTemplate.TemplateSubject))
			{
				config.messageModel.Subject = config.emailTemplate.TemplateSubject;
			}

			if (!string.IsNullOrEmpty(config.emailTemplate.TemplateBody))
			{
				config.messageModel.Body = config.emailTemplate.TemplateBody;
			}

			var result = await emailServices.SendEmail(config.messageModel);

			return result;
		}

	}
}
