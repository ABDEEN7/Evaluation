using Evaluation.Services.Models.SMTP;
using Evaluation.SharedHelper.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.Services.Special
{
	public class EmailServices : IEmailServices
	{
		private readonly LoggingServices loggingServices;

		public EmailServices(LoggingServices loggingServices)
		{
			this.loggingServices = loggingServices;
		}
		public async Task<bool> SendEmail(EmailMessageModel model)
		{

			var mail = new MailMessage
			{
				Subject = model.Subject,
				Body = model.Body,
				From = new MailAddress(model.SenderAddress, model.SenderDisplayName),
				IsBodyHtml = model.IsBodyHTML
			};

			foreach (var toEmail in model.ToEmails)
			{
				mail.To.Add(toEmail);
			}

			foreach (var toEmail in model.CCEmails)
			{
				mail.CC.Add(toEmail);
			}

			foreach (var attachment in model.Attachments)
			{
				mail.Attachments.Add(attachment);

			}
			List<MemoryStream> streams = new List<MemoryStream>();
			if (model.AttachmentArrays != null)
			{
				foreach (var item in model.AttachmentArrays)
				{

					var filestream = new MemoryStream(item);
					streams.Add(filestream);
					filestream.Position = 0;
					var name = Guid.NewGuid().ToString();
					mail.Attachments.Add(new System.Net.Mail.Attachment(filestream, $"{name}.pdf", "application/pdf"));
					//mail.Attachments.Add(new System.Net.Mail.Attachment(filestream, $"test_{name}.docx", MediaTypeNames.Application.Octet));
				}
			}

			var networkCredential = new NetworkCredential(model.UserName, model.Password);

			var smtpClient = new SmtpClient
			{
				Host = model.Host,
				Port = model.Port,
				EnableSsl = model.EnableSSL,
				UseDefaultCredentials = model.UseDefaultCredentials,
				Credentials = networkCredential,
			};

			smtpClient.Timeout = model.EmailRequestTimeout * 1000;

			mail.BodyEncoding = Encoding.Default;
			try
			{
				await smtpClient.SendMailAsync(mail);
			}
			catch (Exception ex)
			{
				foreach (var item in streams)
				{
					item.Close();
				}
				loggingServices.SaveExceptionLog(ex);

				throw new EmailServicesException(ex.Message, ex);

			}
			foreach (var item in streams)
			{
				item.Close();
			}

			return true;
			//stream.Close();

		}

		public async Task<bool> SendTestEmail(params string[] emails)
		{
			if (emails != null && emails.Any())
			{
				var userEmailOptions = new EmailMessageModel
				{
					ToEmails = emails.ToList(),
					Subject = "This is subject test",
					Body = "This is body test",
					SenderAddress = "tawtheef-test@edu.gov.qa",
					SenderDisplayName = "EMAIL TEST FROM LICENSES",
					UserName = "tawtheef-test@edu.gov.qa",
					Password = "",
					Host = "smtp.edu.gov.qa",
					Port = 25,
					EnableSSL = false,
					UseDefaultCredentials = false,
					IsBodyHTML = true,
					EmailRequestTimeout = 100
				};

				return await SendEmail(userEmailOptions);
			}
			return false;
		}

	}
}
