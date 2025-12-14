using Evaluation.DAL.Models.BaseModule;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.DAL.Models.PermissionEntity;


[Index(nameof(BackendName), IsUnique = true)]
public class Permission : EntityBase
{
    public string BackendName { get; set; } = null!;
    public string? NameAr { get; set; }
    public string? NameEn { get; set; }
    public string? Description { get; set; }
    public virtual ICollection<ControlValidation>? ControlValidations { get; set; } = new List<ControlValidation>();
    public virtual ICollection<PagePermission>? PagePermissions { get; set; } = new List<PagePermission>();
    public virtual ICollection<RolePermission>? RolePermissions { get; set; } = new List<RolePermission>();
}
