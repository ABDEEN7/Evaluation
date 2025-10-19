using Evaluation.DAL.Entities.ActionEntities;
using Evaluation.DAL.Entities.Attachments;
using  Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.ServicesEntities;
using Evaluation.DAL.Entities.SystemModulesEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Entities.Logs
{
    public class ActionTransactionsLog:EntityBase
    {
        public Guid ServiceActionId { get; set; }
        public ServiceAction? ServiceAction { get; set; }
        public Guid RefId { get; set; }
        public SystemModule? SystemModule { get; set; }
        public Guid? SystemModuleId { get; set; }
        public Guid PreviousStatusId { get; set; }
        public Guid NextStatusId { get; set; }
        public string? Remarks { get; set; }
        public ICollection<Attachment>? ActionTransactionAttachments { get; set; }
    }
}
