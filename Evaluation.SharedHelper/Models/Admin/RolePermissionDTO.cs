
namespace Evaluation.SharedHelper.Models.Admin;

public  class RolePermissionDTO : EntityBaseDTO
{
    public Guid? RoleId { get; set; }
    public RoleDTO? Role { get; set; }

    public Guid? PermissionId { get; set; }
    public PermissionDTO? Permission { get; set; }

}
