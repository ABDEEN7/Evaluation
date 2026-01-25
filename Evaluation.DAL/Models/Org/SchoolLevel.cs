using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;

namespace Evaluation.DAL.Models.Org;

public class SchoolLevel : EntityBase, IAuditLogEntity
{
    public string? NSISCode { get; set; }
    public int Year { get; set; }
    public Guid SchoolId { get; set; }
    public School? School { get; set; } = null!;
    public Guid EducationLevelId { get; set; }
    public EducationLevel? EducationLevel { get; set; } = null!;
}
