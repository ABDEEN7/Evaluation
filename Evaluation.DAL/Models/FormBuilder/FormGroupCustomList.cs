using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.FormBuilder
{
    public class FormGroupCustomList : EntityBase, IAuditLogEntity
    {
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string BackendName { get; set; } = null!;
        public string Schema { get; set; } = null!;
    }
}
