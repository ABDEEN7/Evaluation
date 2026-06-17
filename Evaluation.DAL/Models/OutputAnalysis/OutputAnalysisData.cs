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
    public class OutputAnalysisData : EntityBase, IAuditLogEntity
    {
        public Guid OutputAnalysisFinalResultId { get; set; }
        public OutputAnalysisFinalResult? OutputAnalysisFinalResult { get; set; }
        public Guid AnalysisTypeId { get; set; }
        public AnalysisType? AnalysisType { get; set; }
        public Guid EvaluationRequestId { get; set; }
        public EvaluationRequest? EvaluationRequest { get; set; }
        public int Grade { get; set; }
        public int LastYear { get; set; }
        public int PreviousYear { get; set; }
        public decimal LastYearValue { get; set; }
        public decimal PreviousYearValue { get; set; }
        public decimal Difference { get; set; }
        public string? SubjectCode { get; set; }
        public string? Track { get; set; }
        public Guid? FormEvalMatrixValueId { get; set; }
        public FormEvalMatrixValue? FormEvalMatrixValue { get; set; }
        public decimal ActualValue { get; set; }
        public string? Note { get; set; }
        public int LastYearStudentCount { get; set; } = 0;
        public int PreviousYearStudentCount { get; set; } = 0;
        public string? TermCode { get; set; }
    }
}
