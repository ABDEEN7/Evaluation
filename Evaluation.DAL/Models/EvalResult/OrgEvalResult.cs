using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.Calendars;
using Evaluation.DAL.Models.Org;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.EvalResult
{
    public class OrgEvalResult : EntityBase, IAuditLogEntity
    {
        public Guid AcademicYearId { get; set; }
        public AcademicYear? AcademicYear { get; set; }

        public Guid OrgTreeId { get; set; }
        public OrgTree? OrgTree { get; set; }
        public Guid DepEvalMatrixId { get; set; }
        public DepEvalMatrix? DepEvalMatrix { get; set; }
        public decimal FinalEvalValue { get; set; } = 0;

    }
}
