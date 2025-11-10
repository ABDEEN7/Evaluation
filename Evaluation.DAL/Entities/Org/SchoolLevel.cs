using  Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Calendars;

namespace Evaluation.DAL.Entities.Org;

public class SchoolLevel : EntityBase
{
    public Guid SchoolId { get; set; }
    public School School { get; set; } = null!;
    public int Year { get; set; }
    public Guid EducationLevelId { get; set; }
    public EducationLevel EducationLevel { get; set; } = null!;
}
