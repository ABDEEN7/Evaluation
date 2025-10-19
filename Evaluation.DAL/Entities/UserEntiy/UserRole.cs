using Evaluation.DAL.Entities.Authentication;
using  Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.PermissionEntity;

namespace Evaluation.DAL.Entities.UserEntiy;

public class UserRole : EntityBase
{
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public Guid RoleId { get; set; }
    public Role? Role { get; set; }
}