using Evaluation.DAL.Entities.Base;

namespace Evaluation.DAL.Entities;

public class DepartmentRoleAttributeValue : EntityBase
{
    public Guid DepartmentId { get; set; }
    public Guid RoleAttributeId { get; set; }
    public string Value { get; set; } = string.Empty;
}
