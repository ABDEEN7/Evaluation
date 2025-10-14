using Evaluation.DAL.Entities.BaseModule;

namespace Evaluation.DAL.Entities.Authentication;

public class RolePermission : EntityBase
{
    public Guid RoleId { get; set; }
    public Role? Role { get; set; }
    public Guid PermissionId { get; set; }
    public Permission? Permission { get; set; }
}
