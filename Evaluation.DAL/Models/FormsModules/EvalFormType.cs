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
    public class EvalFormType : EntityBase, IAuditLogEntity
    {
        public Guid DepartmentId { get; set; }
        public Department? Department { get; set; }
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string BackendName { get; set; } = null!;
        public int OrderNo { get; set; } = 0;
    }
}
