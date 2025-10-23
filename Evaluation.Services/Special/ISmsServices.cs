using Evaluation.Services.Models.SMTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.Services.Special
{
    public interface ISmsServices
    {
        Task<bool> SendMessage(SMSMessageModel model);
    }
}
