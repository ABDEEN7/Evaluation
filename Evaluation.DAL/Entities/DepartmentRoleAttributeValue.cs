using Evaluation.DAL.Entities.AdminPanel;

namespace Evaluation.DAL.Entities;

public class DepartmentRoleAttributeValue : BaseEntities
{
    public int DepartmentId { get; set; }
    public int RoleAttributeId { get; set; }
    public string Value { get; set; }
}
