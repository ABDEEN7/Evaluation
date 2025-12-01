using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.UserEntiy;

namespace Evaluation.DAL.Models.PermissionEntity;

public class Role : EntityBase
{
    public required string NameAr { get; set; }
    public required string NameEn { get; set; }
    public List<UserRole> UserRoles { get; set; } = new();
    public List<RolePermission> RolePermission { get; set; } = new();
}
