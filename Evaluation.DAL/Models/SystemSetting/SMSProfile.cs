using Evaluation.DAL.Models.BaseModule;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.DAL.Models.SystemSetting;
[Index(nameof(BackendName), IsUnique = true)]

public class SMSProfile: EntityBase
{
    public string BackendName { get; set; } = null!;
    public string BaseUrl { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool IsDefault { get; set; }
}
