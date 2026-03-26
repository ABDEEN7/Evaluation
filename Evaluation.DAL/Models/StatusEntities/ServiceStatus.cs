using Evaluation.DAL.Models.ActionEntities;
using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.ServiceEnities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.StatusEntities
{
    public class ServiceStatus : EntityBase, IAuditLogEntity
    {
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string BackendName { get; set; } = null!;
        public bool IsInitial { get; set; }
        public Guid ServiceStatusTypeId { get; set; }
        public ServiceStatusType? ServiceStatusType { get; set; }
        public string? ColorCode { get; set; }
        public int? OrderNo { get; set; }
     
        public Guid ServiceId { get; set; }
        public Service Service { get; set; } = null!;
        public Guid StatusGroupId { get; set; }

        public virtual ICollection<ActionStatusConfiguration>? NextStatusConfigurations { get; } = new List<ActionStatusConfiguration>();
        public virtual ICollection<ActionStatusConfiguration>? CurrentStatusConfigurations { get; } = new List<ActionStatusConfiguration>();
        public virtual ICollection<ServiceStatusPartyTypeDisplayName>? StatusPartyTypeDisplayNames { get; } = new List<ServiceStatusPartyTypeDisplayName>();
        public virtual ICollection<ServiceStatusPreventPartyType>? StatusPreventPartyTypes { get; } = new List<ServiceStatusPreventPartyType>();

    }
}
