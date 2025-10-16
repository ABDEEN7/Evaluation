using Evaluation.DAL.Entities.Audit;
using Evaluation.DAL.Entities.Base;
using Evaluation.DAL.Entities.Authentication;
using Evaluation.DAL.Entities.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Entities.ActionEntities
{
    public class ActionStatusConfigNotification : BaseEntities, IAuditLogEntity
    {
        public Guid ActionStatusConfigurationId { get; set; }
        public ActionStatusConfiguration? ActionStatusConfiguration { get; set; }

        public Guid PartyTypeId { get; set; }
        public PartyType? PartyType { get; set; }
        public bool IsEmailSend { get; set; }
        public Guid? EmailTemplateId { get; set; }
        public EmailTemplate? EmailTemplate { get; set; }
        public bool IsMessageSend { get; set; }
        public Guid? SMSTemplateId { get; set; }
        public SMSTemplate? SMSTemplate { get; set; }
        public bool IsNotificationSend { get; set; }
        public Guid? NotificationTemplateId { get; set; }
        public NotificationTemplate? NotificationTemplate { get; set; }
    }
}
