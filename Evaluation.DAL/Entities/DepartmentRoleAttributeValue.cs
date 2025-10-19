using Evaluation.DAL.Entities.BaseModule;

namespace Evaluation.DAL.Entities;

public class DepartmentRoleAttributeValue : EntityBase
{
    public Guid DepartmentId { get; set; }
    public Guid RoleAttributeId { get; set; }
    public string Value { get; set; } = string.Empty;
}
