using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.Services.Models.SMTP
{
    public class SMSMessageModel
    {
        public string mobile { get; set; } = null!;
        public string message { get; set; } = null!;
        public string BaseUrl { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public Guid? RefId { get; set; }

    }
}
