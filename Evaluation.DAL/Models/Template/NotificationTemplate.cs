using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.ServiceEnities;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.DAL.Models.Template;
[Index(nameof(BackendName), IsUnique = true)]
public class NotificationTemplate : EntityBase
{
   public string? SubjectAr { get; set; }
    public string? SubjectEn { get; set; }
    public string BodyAr { get; set; } = "";
    public string BodyEn { get; set; } = "";
    public string BackendName { get; set; } = "";
    public Guid? SystemModuleId { get; set; }
    public SystemModule? SystemModule { get; set; }
    //public IList<ActionStatusConfigNotification>? Notifications { get; set; }
    public Guid? ServiceId { get; set; }
    public Service? Service { get; set; }
    public Guid? DepartmentId { get; set; }
    public Department? Department { get; set; }
}
