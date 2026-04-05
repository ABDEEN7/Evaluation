using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.StatusEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.FormsModules
{
    public class PartyTypeEvalPartyStatus : EntityBase, IAuditLogEntity
    {
        public Guid PartyTypeEvalPartyId { get; set; }
        public PartyTypeEvalParty? PartyTypeEvalParty { get; set; }

        public Guid ServiceStatusId { get; set; }
        public ServiceStatus? ServiceStatus { get; set; }

    }
}
