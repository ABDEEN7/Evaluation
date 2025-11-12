
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.TemplatesDTO
{
    public class EmailTemplateDTO : EntityBaseDTO
    {
        public string TitleAr { get; set; } = null!;
        public string TitleEn { get; set; } = null!;
        public string TemplateSubject { get; set; } = null!;
        public string TemplateBody { get; set; } = null!;
        public string BackendName { get; set; } = null!;
        public Guid EmailProfileId { get; set; }
        public string EmailProfile { get; set; } = null!;
        public Guid? ServiceId { get; set; }
        public string? Service { get; set; }
        public string? fielsFromRequest { get; set; }
        public string? fielsFromScholarship { get; set; }

    }
}
