using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.OutputAnalysis
{
    public class OutputAnalysisFinalResult : EntityBase, IAuditLogEntity
    {
        public Guid AnalysisTypeId { get; set; }
        public AnalysisType? AnalysisType { get; set; }
        public Guid EvaluationRequestId { get; set; }
        public EvaluationRequest? EvaluationRequest { get; set; }
        public decimal ActualValue { get; set; }
        public Guid? FormEvalMatrixValueId { get; set; }
        public FormEvalMatrixValue? FormEvalMatrixValue { get; set; }
        public string? Note { get; set; }
    }
}
