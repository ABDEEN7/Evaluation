using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.FormsModules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.OutputAnalysis
{
    public class AnalysisType : EntityBase, IAuditLogEntity
    {
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string BackendName { get; set; } = null!;
        public string? Grades { get; set; }
        public string? SubjectCode { get; set; } // CourceCode
        public string? AnalysisConfig { get; set; }
        public Guid? FormEvalMatrixId { get; set; }
        public FormEvalMatrix? FormEvalMatrix { get; set; }
        public int OrderNo { get; set; } = 0;
    }
}
