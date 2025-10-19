using Evaluation.DAL.Entities.Audit;
using  Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Entities.StatusEntities
{
    public class ServiceStatusPreventPartyType : EntityBase, IAuditLogEntity
    {
        public Guid StatusId { get; set; }
        public  ServiceStatus Status { get; set; }
        public Guid PartyTypeId { get; set; }
        public  PartyType PartyType { get; set; }

    }
}
