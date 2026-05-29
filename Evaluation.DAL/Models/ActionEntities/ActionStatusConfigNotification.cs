using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.Template;
using Evaluation.DAL.Models.UserEntiy;

namespace Evaluation.DAL.Models.ActionEntities
{
    public class ActionStatusConfigNotification : EntityBase, IAuditLogEntity
    {
        public Guid ActionStatusConfigurationId { get; set; }
        public ActionStatusConfiguration? ActionStatusConfiguration { get; set; }

        public Guid PartyTypeId { get; set; }
        public PartyType? PartyType { get; set; }
        public bool IsEmailSend { get; set; }
        public Guid? EmailTemplateId { get; set; }
        public EmailTemplate? EmailTemplate { get; set; }
        public int EmailMinsFromAction { get; set; } = 0;
        public bool IsMessageSend { get; set; }
        public Guid? SMSTemplateId { get; set; }
        public SMSTemplate? SMSTemplate { get; set; }
        public int SMSMinsFromAction { get; set; } = 0;
        public bool IsNotificationSend { get; set; }
        public Guid? NotificationTemplateId { get; set; }
        public NotificationTemplate? NotificationTemplate { get; set; }
        public int NotificationMinsFromAction { get; set; } = 0;

    }
}
