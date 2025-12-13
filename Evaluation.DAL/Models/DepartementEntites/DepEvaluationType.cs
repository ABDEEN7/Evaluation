using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.DepartementEntites
{
    public class DepEvaluationType : EntityBase,IAuditLogEntity
    {
        public Guid DepartmentId { get; set; }
        public Department? Department { get; set; }
        public string BackendName { get; set; } = null!;
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public int OrderNo { get; set; }
    }
}
