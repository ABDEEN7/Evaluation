using Evaluation.DAL.Entities.AdminPanel;

namespace Evaluation.DAL.Entities.Authentication;

public class RolePermission : BaseEntities
{
    public Guid RoleId { get; set; }
    public Role? Role { get; set; }
    public Guid PermissionId { get; set; }
    public Permission? Permission { get; set; }
}
