using Evaluation.DAL.Entities.Audit;
using  Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Entities.ServicesEntities
{
    public class ServiceInitiatorPartyType : EntityBase, IAuditLogEntity
    {
        public Guid serviceId { get; set; }
        public Service service { get; set; }
        public Guid PartyTypeId { get; set; }
        public PartyType PartyType { get; set; }

    }
}
