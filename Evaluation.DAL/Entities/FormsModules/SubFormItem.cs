using Evaluation.DAL.Entities.Audit;
using Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.FormBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Entities.FormsModules
{
    public class SubFormItem : EntityBase , IAuditLogEntity
    {
        public Guid FormItemId { get; set; }
        public FormItem? FormItem { get; set; }
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;

        public bool IsOption { get; set; }

        public Guid? DropDownTypeId { get; set; }
        public DropDownType? DropDownType { get; set; }
    }
}
