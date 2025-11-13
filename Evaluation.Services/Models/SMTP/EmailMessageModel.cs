using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.Services.Models.SMTP
{
	public class EmailMessageModel
	{

		public EmailMessageModel()
		{
			ToEmails = new List<string>();
			CCEmails = new List<string>();
			Attachments = new List<System.Net.Mail.Attachment>();
		}
		public List<string> ToEmails { get; set; }
		public string Subject { get; set; }
		public string SenderMail { get; set; }
		public string Body { get; set; }
		public List<string> CCEmails { get; set; }
		public Guid? RefId { get; set; }
		public string ModuleBackendName { get; set; }
		public List<System.Net.Mail.Attachment> Attachments { get; set; }
		public List<byte[]>? AttachmentArrays { get; set; }

		//--------------------------------------------------------------------------------------

		public string SenderAddress { get; set; }
		public string SenderDisplayName { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string Host { get; set; }
		public int Port { get; set; }
		public bool EnableSSL { get; set; }
		public bool UseDefaultCredentials { get; set; }
		public bool IsBodyHTML { get; set; }
		public int EmailRequestTimeout { get; set; }


	}
}
