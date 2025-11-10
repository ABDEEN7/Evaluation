using Scholarship.SharedHelper.Models.Admin;


namespace Evaluation.SharedHelper.Models.Api.ActionEntitiesDTOs
{
    public class ActionStatusConfigurationDTO : EntityBaseDTO
    {
        public Guid? ServiceId { get; set; }
        public Guid ServiceActionId { get; set; }
        public string ServiceAction { get; set; } = null!;
        public Guid CurrentStatusId { get; set; }
        public string CurrentStatus { get; set; } = null!;
        public Guid NextStatusId { get; set; }
        public string NextStatus { get; set; } = null!;
        public bool IsRemark { get; set; }
        public bool IsAuto { get; set; } = false;
        public string? RemarkLabel { get; set; }
        public string? RemarkLabelAr { get; set; }
        public string? AttachmentLabel { get; set; }
        public string? AttachmentLabelAr { get; set; }
        public string? RemarkLabelEn { get; set; }
        public string? AttachmentLabelEn { get; set; }
        public bool IsRemarkRequired { get; set; }
        public bool IsOtherAttachment { get; set; }
        public bool IsOtherAttachmentRequired { get; set; }
        public int OrderNo { get; set; }
        public int? NotificationCount { get; set; }
        public bool ShowIsDefaultAssigner { get; set; }
    }
}
