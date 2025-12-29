using Evaluation.DAL.Models.ActionEntities;
using Evaluation.DAL.Models.Attachments;
using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Models.ServiceRequestEntities;
using Evaluation.DAL.Models.StatusEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.SystemLog
{
    public class EvalRequestActionTransactionsLog : EntityBase , IAuditLogEntity
    {
        public Guid ServiceActionId { get; set; }
        public ServiceAction? ServiceAction { get; set; }
        public Guid EvaluationRequestId { get; set; }
        public EvaluationRequest? EvaluationRequest { get; set; }
        public Guid RefId { get; set; }
        public SystemModule? SystemModule { get; set; }
        public Guid? SystemModuleId { get; set; }
        public Guid PreviousStatusId { get; set; }
        //public ServiceStatus? PreviousStatus { get; set; }
        public Guid NextStatusId { get; set; }
        //public ServiceStatus? NextStatus { get; set; }
        public string? Remarks { get; set; }
        public virtual ICollection<EvalAttachment>? EvalAttachments { get; set; }
    }
}
