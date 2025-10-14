using Evaluation.DAL.Entities.BaseModule;

namespace Evaluation.DAL.Entities.Authentication;

public class UserRole : EntityBase
{
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public Guid RoleId { get; set; }
    public Role? Role { get; set; }
}