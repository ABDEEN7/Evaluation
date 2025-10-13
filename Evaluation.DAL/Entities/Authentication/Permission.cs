using Evaluation.DAL.Entities.BaseModule;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.DAL.Entities.Authentication;


[Index(nameof(BackendName), IsUnique = true)]
public class Permission : BaseEntities
{
    public string BackendName { get; set; } = null!;
    public string? NameAr { get; set; }
    public string? NameEn { get; set; }
    public string? Description { get; set; }
    public ICollection<ControlValidation> ControlValidations { get; set; } = new List<ControlValidation>();
    public ICollection<PagePermission> PagePermissions { get; set; } = new List<PagePermission>();
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
