using Evaluation.DAL.Entities.Audit;
using Evaluation.DAL.Entities.Base;
using Evaluation.DAL.Entities.StatusEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Entities.ActionEntities
{
    public class ActionStatusConfiguration : BaseEntities, IAuditLogEntity
    {
        public Guid ServiceActionId { get; set; }
        public ServiceAction? ServiceAction { get; set; }

        public Guid CurrentStatusId { get; set; }
        public ServiceStatus? CurrentStatus { get; set; }

        public Guid NextStatusId { get; set; }
        public ServiceStatus? NextStatus { get; set; }

        public bool IsRemark { get; set; }
        public bool IsAuto { get; set; } = false;

        public string? RemarkLabelAr { get; set; }
        public string? AttachmentLabelAr { get; set; }
        public string? RemarkLabelEn { get; set; }
        public string? AttachmentLabelEn { get; set; }

        public bool IsRemarkRequired { get; set; }
        public bool IsOtherAttachment { get; set; }
        public bool IsOtherAttachmentRequired { get; set; }
        public int OrderNo { get; set; }

        public IList<ActionStatusConfigNotification> Notifications { get; set; } = new List<ActionStatusConfigNotification>();

        public bool ShowIsDefaultAssigner { get; set; }
    }
}
