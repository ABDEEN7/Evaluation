using Evaluation.Services.Models.SMTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.Services.Special
{
	public interface IEmailServices
	{
		Task<bool> SendEmail(EmailMessageModel model);
		Task<bool> SendTestEmail(params string[] emails);
	}
}
