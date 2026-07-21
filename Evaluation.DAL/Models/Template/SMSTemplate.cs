using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.ServiceEnities;
using Evaluation.DAL.Models.SystemSetting;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.DAL.Models.Template;
[Index(nameof(BackendName), IsUnique = true)]
public class SMSTemplate: EntityBase
{
    public string TitleAr { get; set; } = string.Empty;
    public string TitleEn { get; set; } = string.Empty;
    public string Messages { get; set; } = null!;
    public string BackendName { get; set; } = null!;
    public Guid SMSProfileId { get; set; }
    public SMSProfile SMSProfile { get; set; } = new();
    public Guid? ServiceId { get; set; }
    public Service? Service { get; set; }
    public Guid? DepartmentId { get; set; }
    public Department? Department { get; set; }
}
