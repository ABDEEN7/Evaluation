using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.Master
{
    public class Country : EntityBase, IAuditLogEntity
    {
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string? CodeAlpha { get; set; }
        public string Currency { get; set; } = null!;
        public int? OrderNo { get; set; }
        public string BackendName { get; set; } = null!;
        public string ISOCode { get; set; } = null!;
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public string? LogoAr { get; set; }
        public string? LogoEn { get; set; }
    }
}
