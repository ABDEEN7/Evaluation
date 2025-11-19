using Evaluation.DAL.Models.BaseModule;

namespace Evaluation.DAL.Models.Org;

public class SchoolLevel : EntityBase
{
    public int Year { get; set; }
    public Guid SchoolId { get; set; }
    public OrgTree School { get; set; } = null!;
    public Guid EducationLevelId { get; set; }
    public EducationLevel EducationLevel { get; set; } = null!;
}
