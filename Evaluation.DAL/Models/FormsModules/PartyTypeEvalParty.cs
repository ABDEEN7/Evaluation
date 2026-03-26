using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.StatusEntities;
using Evaluation.DAL.Models.UserEntiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.FormsModules
{
    public class PartyTypeEvalParty : EntityBase, IAuditLogEntity
    {
        public Guid PartyTypeId { get; set; }
        public PartyType? PartyType { get; set; }
        public Guid EvaluationPartyId { get; set; }
        public EvaluationParty? EvaluationParty { get; set; }
    }
}
