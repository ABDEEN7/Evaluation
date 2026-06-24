using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;

namespace Evaluation.DAL.Models.DepartementEntites;

public class DepartmentRoleAttributeValue : EntityBase,IAuditLogEntity
{
    public Guid DepartmentId { get; set; }
    public Department? Department { get; set; }
    public Guid SystemAttributeId { get; set; }
    public SystemAttribute? SystemAttribute { get; set; }
    public string? Value { get; set; }
    public string? AttributeConfig { get; set; }
}
