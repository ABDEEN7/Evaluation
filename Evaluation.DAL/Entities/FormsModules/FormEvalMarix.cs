using Evaluation.DAL.Entities.Audit;
using Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.DepartementEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Entities.FormsModules
{
    public class FormEvalMarix :EntityBase,IAuditLogEntity
    {
        public Guid DepartmentId { get; set; }
        public Department? Department { get; set; }
        public string BackenName { get; set; } = null!;
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public DateOnly Startdate { get; set; }
        public DateOnly? EndDate { get; set; }
        public int OrderNo { get; set; }

    }
}
