using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.ActionEntities
{
    public class ActionTemplateDoc : EntityBase, IAuditLogEntity
    {
        public Guid ServiceActionId { get; set; }
        public ServiceAction? ServiceAction { get; set; }
        public Guid TemplateDocId { get; set; }
        public TemplateDocument? TemplateDoc { get; set; }
    }
}
