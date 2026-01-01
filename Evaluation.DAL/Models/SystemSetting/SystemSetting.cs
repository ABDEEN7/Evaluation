using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.DepartementEntites;

namespace Evaluation.DAL.Models.SystemSetting;

public class SystemSetting : EntityBase,IAuditLogEntity
{
    public Guid? DepartmentId { get; set; }
    public Department? Department { get; set; }
    public string SettingGroup { get; set; } = null!;
    public string SettingKey { get; set; } = null!;

    public string SettingValue { get; set; } = null!;

    public string? Description { get; set; }
}