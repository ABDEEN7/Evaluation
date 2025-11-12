

namespace Evaluation.SharedHelper.Models.Api.ActionEntitiesDTOs
{
    public class ActionStatusConfigNotificationDTO : EntityBaseDTO
    {
        public Guid ActionStatusConfigurationId { get; set; }
        public string? ActionStatusConfiguration { get; set; }
        public Guid PartyTypeId { get; set; }
        public string PartyType { get; set; } = null!;
        public bool IsEmailSend { get; set; }
        public Guid? EmailTemplateId { get; set; }
        public string? EmailTemplate { get; set; }
        public bool IsMessageSend { get; set; }
        public Guid? SMSTemplateId { get; set; }
        public string? SMSTemplate { get; set; }
        public bool IsNotificationSend { get; set; }
        public Guid? NotificationTemplateId { get; set; }
        public string? NotificationTemplate { get; set; }
    }
}
