using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.FormsModules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.Planing.EvaluationRequestEntity
{
    public class EvalRequestAssignmentScope : EntityBase, IAuditLogEntity
    {
        public Guid EvaluationRequestAssignmentId { get; set; }
        public EvaluationRequestAssignment? EvaluationRequestAssignment { get; set; }

        public Guid ScopeId { get; set; }
        public Scope? Scope { get; set; }
        public string? Note { get; set; }
    }
}
