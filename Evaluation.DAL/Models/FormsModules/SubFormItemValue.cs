using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.FormBuilder;
using Evaluation.DAL.Models.UserEntiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.FormsModules
{
    public class SubFormItemValue : EntityBase, IAuditLogEntity
    {
        public Guid SubFormItemId { get; set; }
        public SubFormItem? SubFormItem { get; set; }
        public Guid UserId { get; set; }
        public MinistryUser? User { get; set; }
        public Guid? FieldDropDownValueId { get; set; }
        public FieldDropDownValue? FieldDropDownValue { get; set; }
        public string? Note { get; set; }
    }
}
