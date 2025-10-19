using Evaluation.DAL.Entities.Audit;
using  Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Entities.ActionEntities
{
    public class ActionAssignPartyType : EntityBase, IAuditLogEntity
    {
        public Guid EvaluationActionId { get; set; }
        public ServiceAction? EvaluationAction { get; set; }
        public Guid PartyTypeId { get; set; }
        public PartyType? PartyType { get; set; }
        public int MaximumAssignedUser { get; set; }

    }
}
