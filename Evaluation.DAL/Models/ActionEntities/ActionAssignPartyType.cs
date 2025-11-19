using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.UserEntiy;

namespace Evaluation.DAL.Models.ActionEntities
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
