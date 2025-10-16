using Evaluation.DAL.Entities.Base;

namespace Evaluation.DAL.Entities.PermissionEntity;

public class RolePermission : EntityBase
{
    public Guid RoleId { get; set; }
    public Role? Role { get; set; }
    public Guid PermissionId { get; set; }
    public Permission? Permission { get; set; }
}
