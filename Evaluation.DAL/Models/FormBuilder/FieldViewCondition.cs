using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.FormBuilder
{
    public class FieldViewCondition : EntityBase, IAuditLogEntity
    {
        public Guid FieldId { get; set; }
        public Field? Field { get; set; }
        public string operators { get; set; } = null!;
        public string FieldValue { get; set; } = null!;
        public bool IsSufficient { get; set; }
        public string? FieldDropDownValueIds { get; set; }
        public Guid? ParentFieldId { get; set; }
        public Field? ParentField { get; set; }
    }
}
