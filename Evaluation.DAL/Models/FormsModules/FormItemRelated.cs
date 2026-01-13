using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.FormsModules
{
    public class FormItemRelated : EntityBase, IAuditLogEntity
    {
        public Guid FormItemId { get; set; }
        public FormItem? FormItem { get; set; }

        public Guid RelatedItemId { get; set; }
        public FormItem? RelatedItem { get; set; }
    }
}
