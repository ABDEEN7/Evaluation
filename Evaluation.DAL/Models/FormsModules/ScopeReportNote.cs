using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.FormsModules
{
    public class ScopeReportNote : EntityBase, IAuditLogEntity
    {
        public Guid EvaluationRequestId { get; set; }
        public EvaluationRequest? EvaluationRequest { get; set; }
        public Guid DepartmentId { get; set; }
        public Department? Department { get; set; }
        public Guid ScopeId { get; set; }
        public Scope? Scope { get; set; }
        public string PositivePoint { get; set; } = null!;
        public string NegativePoint { get; set; } = null!;
        public string? Note { get; set; }
    }
}
