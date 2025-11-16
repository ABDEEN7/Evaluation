using Evaluation.DAL.Models.BaseModule;
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
}
