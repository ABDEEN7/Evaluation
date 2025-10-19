using Evaluation.DAL.Entities.Audit;
using  Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Entities.ActionEntities
{
    public class ActionTemplateDoc : EntityBase, IAuditLogEntity
    {
        public Guid ServiceActionId { get; set; }
        public ServiceAction? ServiceAction { get; set; }
        public Guid TemplateDocId { get; set; }
        public TemplateDocument? TemplateDoc { get; set; }
    }
}
