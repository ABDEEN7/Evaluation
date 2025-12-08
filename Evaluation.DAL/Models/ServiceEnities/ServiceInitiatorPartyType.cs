using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.UserEntiy;

namespace Evaluation.DAL.Models.ServiceEnities
{
    public class ServiceInitiatorPartyType : EntityBase, IAuditLogEntity
    {
        public Guid serviceId { get; set; }
        public Service? service { get; set; }
        public Guid PartyTypeId { get; set; }
        public PartyType? PartyType { get; set; }

    }
}
