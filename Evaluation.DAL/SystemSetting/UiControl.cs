using Evaluation.DAL.Entities.Base;

namespace Evaluation.DAL.SystemSetting;
[Index(nameof(BackendName), IsUnique = true)]
public class UiControl : EntityBase
{
    public string PageName { get; set; } = null!;
    public string UserUiname { get; set; } = null!;

    public string BackendName { get; set; } = null!;

    public string ControlName { get; set; } = null!;

    public string? ValueEn { get; set; }

    public string? ValueAr { get; set; }

    public string? Url { get; set; }
}
