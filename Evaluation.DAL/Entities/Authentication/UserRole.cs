using Evaluation.DAL.Entities.AdminPanel;

namespace Evaluation.DAL.Entities.Authentication;

public class UserRole : BaseEntities
{
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public Guid RoleId { get; set; }
    public Role? Role { get; set; }
}