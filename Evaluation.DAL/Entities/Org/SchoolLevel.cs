using Evaluation.DAL.Entities.BaseModule;

namespace Evaluation.DAL.Entities.Org;

public class SchoolLevel : EntityBase
{
    public int SchoolId { get; set; }
    public School School { get; set; } = null!;
    public int LevelId { get; set; }
    public Level Level { get; set; } = null!;
}
