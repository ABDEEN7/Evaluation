using Evaluation.DAL.Entities.Audit;
using Evaluation.DAL.Entities.Authentication;
using Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.FormBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Entities.FormsModules
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
