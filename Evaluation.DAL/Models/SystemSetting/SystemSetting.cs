using Evaluation.DAL.Models.BaseModule;

namespace Evaluation.DAL.Models.SystemSetting;

public class SystemSetting : EntityBase
{
    public string SettingGroup { get; set; } = null!;
    public string SettingKey { get; set; } = null!;

    public string SettingValue { get; set; } = null!;

    public string? Description { get; set; }
}