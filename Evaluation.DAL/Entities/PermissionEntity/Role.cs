using  Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.UserEntiy;

namespace Evaluation.DAL.Entities.PermissionEntity;

public class Role : EntityBase
{
    public required string NameAr { get; set; }
    public required string NameEn { get; set; }
    public List<UserRole> UserRoles { get; set; } = new();
}
