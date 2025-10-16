using Evaluation.DAL.Entities.Audit;
using Evaluation.DAL.Entities.Base;
using Evaluation.DAL.Entities.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Entities.StatusEntities
{
    public class ServiceStatusPartyTypeDisplayName : BaseEntities, IAuditLogEntity
    {
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public Guid StatusId { get; set; }
        public  ServiceStatus Status { get; set; }
        public Guid PartyTypeId { get; set; }
        public  PartyType PartyType { get; set; }
    }
}
