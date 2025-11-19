using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.PermissionEntity;

namespace Evaluation.DAL.Models.UserEntiy;

public class UserRole : EntityBase
{
    public Guid UserId { get; set; }
    public MinistryUser? User { get; set; }
    public Guid RoleId { get; set; }
    public Role? Role { get; set; }
}