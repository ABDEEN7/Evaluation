using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.Calendars;
using Evaluation.DAL.Models.DepartementEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.EvalResult
{
    public class DepEvalMatrix : EntityBase, IAuditLogEntity
    {
        public Guid AcademicYearId { get; set; }
        public AcademicYear? AcademicYear { get; set; }
        public string BackendName { get; set; } = null!;
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public decimal? ItemValue { get; set; }
        public decimal MinValue { get; set; }
        public decimal MaxValue { get; set; }
        public bool RequiredFollowUp { get; set; }
        public int NextFollowUpDays { get; set; } = 0;
        public int NextEvaluationDays { get; set; } = 0;
    }
}
