using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.UserEntiy;

namespace Evaluation.DAL.Models.StatusEntities
{
    public class ServiceStatusPreventPartyType : EntityBase, IAuditLogEntity
    {
        public Guid StatusId { get; set; }
        public  ServiceStatus Status { get; set; }
        public Guid PartyTypeId { get; set; }
        public  PartyType PartyType { get; set; }

    }
}
