using  Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Calendars;

namespace Evaluation.DAL.Entities.Org;

public class SchoolLevel : EntityBase
{
    public int Year { get; set; }
    public Guid SchoolId { get; set; }
    public OrgTree School { get; set; } = null!;
    public Guid EducationLevelId { get; set; }
    public EducationLevel EducationLevel { get; set; } = null!;
}
