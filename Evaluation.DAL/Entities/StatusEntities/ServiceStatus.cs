using Evaluation.DAL.Entities.ActionEntities;
using Evaluation.DAL.Entities.Audit;
using Evaluation.DAL.Entities.Base;
using Evaluation.DAL.Entities.FormBuilder;
using Evaluation.DAL.Entities.ServicesEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Entities.StatusEntities
{
    public class ServiceStatus : BaseEntities, IAuditLogEntity
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string BackendName { get; set; }
        public bool IsInitial { get; set; }
        public bool IsOpen { get; set; }
        public string? ColorCode { get; set; }
        public int? OrderNo { get; set; }
     
        public Guid ServiceId { get; set; }
        public Service Service { get; set; } = null!;
        public Guid StatusGroupId { get; set; }

        public ICollection<ActionStatusConfiguration> NextStatusConfigurations { get; } = new List<ActionStatusConfiguration>();
        public ICollection<ActionStatusConfiguration> CurrentStatusConfigurations { get; } = new List<ActionStatusConfiguration>();
        public ICollection<ServiceStatusPartyTypeDisplayName> StatusPartyTypeDisplayNames { get; } = new List<ServiceStatusPartyTypeDisplayName>();
        public ICollection<ServiceStatusPreventPartyType> StatusPreventPartyTypes { get; } = new List<ServiceStatusPreventPartyType>();

    }
}
