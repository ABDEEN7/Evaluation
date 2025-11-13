

namespace Evaluation.SharedHelper.Models.Api.TemplatesDTO
{
    public class NotificationDTO : EntityBaseDTO
    {
        public Guid? NotificationTemplateId { get; set; }
        public string? NotificationTemplate { get; set; }
        public string TitleAr { get; set; } = null!;
        public string TitleEn { get; set; } = null!;
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public long ReadCount { get; set; }
        public Guid UserProfileId { get; set; }
        public string UserProfile { get; set; } = null!;
        public DateTime? FirstReadDate { get; set; }
        public DateTime? LastReadDate { get; set; }
        public string? Title { get; set; } = null!;
        public string? Description { get; set; } = null!;
        public Guid? UserId { get; set; } = null!;
        public string? IssuedDate { get; set; } = null!;

    }
}
