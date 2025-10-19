using Microsoft.EntityFrameworkCore;

namespace Evaluation.DAL.Entities.Template;
[Index(nameof(BackendName), IsUnique = true)]
public class NotificationTemplate
{
   public string? SubjectAr { get; set; }
    public string? SubjectEn { get; set; }
    public string BodyAr { get; set; } = "";
    public string BodyEn { get; set; } = "";
    public string BackendName { get; set; } = "";
    public Guid? SystemModuleId { get; set; }
    public SystemModule? SystemModule { get; set; }
    //public IList<ActionStatusConfigNotification>? Notifications { get; set; }
}
