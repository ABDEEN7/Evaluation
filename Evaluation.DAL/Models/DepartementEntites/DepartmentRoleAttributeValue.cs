using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;

namespace Evaluation.DAL.Models.DepartementEntites;

public class DepartmentRoleAttributeValue : EntityBase,IAuditLogEntity
{
    public Guid DepartmentId { get; set; }
    public Guid DepartmentRoleAttributeId { get; set; }
    public DepartmentRoleAttribute? DepartmentRoleAttribute { get; set; }
    public string? Value { get; set; } 
}
