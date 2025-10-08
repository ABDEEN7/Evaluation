using Evaluation.DAL.Entities.AdminPanel;

namespace Evaluation.DAL.Entities.Authentication;

public class Permission : BaseEntities
{
    public string BackEndName { get; set; }
    public string? NameAr { get; set; }
    public string? NameEn { get; set; }
    public string? Description { get; set; }
    public ICollection<ControlValidation> ControlValidations { get; set; } = new List<ControlValidation>();
    public ICollection<PagePermission> PagePermissions { get; set; } = new List<PagePermission>();
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
