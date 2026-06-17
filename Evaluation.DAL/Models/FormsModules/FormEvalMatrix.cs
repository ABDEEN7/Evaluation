using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.DepartementEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.FormsModules
{
    public class FormEvalMatrix :EntityBase,IAuditLogEntity
    {
        public Guid DepartmentId { get; set; }
        public Department? Department { get; set; }
        public string BackenName { get; set; } = null!;
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public DateOnly Startdate { get; set; }
        public DateOnly? EndDate { get; set; }
        public int OrderNo { get; set; }
		public virtual ICollection<FormEvalMatrixValue>? FormEvalMatrixValues { get; set; }


	}
}
