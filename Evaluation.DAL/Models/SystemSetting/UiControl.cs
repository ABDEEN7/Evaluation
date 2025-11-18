using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.DepartementEntites;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.DAL.Models.SystemSetting;
[Index(nameof(BackendName), IsUnique = true)]
public class UiControl : EntityBase
{
    public Guid? DepartmentId { get; set; }
    public Department? Department { get; set; }
    public string PageName { get; set; } = null!;
    public string UserUiname { get; set; } = null!;

    public string BackendName { get; set; } = null!;

    public string ControlName { get; set; } = null!;

    public string? ValueEn { get; set; }

    public string? ValueAr { get; set; }

    public string? Url { get; set; }
}
