using Evaluation.DAL.Entities.Authentication;

namespace Evaluation.DAL.Entities.Template;

public class Notification
{
    public Guid? NotificationTemplateId { get; set; }
    public NotificationTemplate? NotificationTemplate { get; set; }

    public string TitleAr { get; set; } = null!;

    public string TitleEn { get; set; } = null!;

    public string? DescriptionAr { get; set; }

    public string? DescriptionEn { get; set; }

    public long ReadCount { get; set; }


    public Guid UserId { get; set; }
    public User User { get; set; }
    public DateTime? FirstReadDate { get; set; }
    public DateTime? LastReadDate { get; set; }
}
