using Evaluation.DAL.Entities.BaseModule;

namespace Evaluation.DAL.SystemSetting;

public class SystemSetting : BaseEntities
{
    public string SettingGroup { get; set; } = null!;
    public string SettingKey { get; set; } = null!;

    public string SettingValue { get; set; } = null!;

    public string? Description { get; set; }
}