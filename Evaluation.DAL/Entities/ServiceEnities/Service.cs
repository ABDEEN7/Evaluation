using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evaluation.DAL.Entities.SystemModulesEntities;
using Evaluation.DAL.Entities.FormBuilder;
using  Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.StatusEntities;
using Evaluation.DAL.Entities.Audit;

namespace Evaluation.DAL.Entities.ServicesEntities
{
    public class Service : EntityBase, IAuditLogEntity, ILocalized
    {

        public Guid SystemModuleId { get; set; }
        public SystemModule? SystemModule { get; set; }
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string BackendName { get; set; } = null!;
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public string PrefixCode { get; set; } = null!;
        public string ReqNumberDef { get; set; } = null!;
        public bool? IsAutoAssignEnabled { get; set; }
        public bool IsFreez { get; set; }
        public DateTime? FreezDate { get; set; }
        public string? Icon { get; set; }
        public int OrderNo { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? ServiceSettings { get; set; }
        public bool Initialservice { get; set; }

        public string? UrlAr { get; set; }
        public string? UrlEn { get; set; }
        public bool ShowInWebSite { get; set; }

        public virtual ICollection<FormGroup>? FormGroups { get; set; }
        public virtual ICollection<ServiceStatus>? Statuses { get; set; }
        public virtual ICollection<ServiceRequestShowPartyType>? RequestShowPartyType { get; set; }

    }
}
