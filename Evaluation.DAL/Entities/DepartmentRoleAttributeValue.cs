using Evaluation.DAL.Entities.AdminPanel;

namespace Evaluation.DAL.Entities;

public class DepartmentRoleAttributeValue : BaseEntities
{
    public Guid DepartmentId { get; set; }
    public Guid RoleAttributeId { get; set; }
    public string Value { get; set; } = string.Empty;
}
