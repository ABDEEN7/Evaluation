
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.TemplatesDTO
{
    public class SMSTemplateDTO:EntityBaseDTO
    {
        public string TitleAr { get; set; } = null!;
        public string TitleEn { get; set; } = null!;
        public string Messages { get; set; } = null!;
        public string BackendName { get; set; } = null!;
        public Guid SMSProfileId { get; set; }
        public string SMSProfile { get; set; } = null!;
    }
}
