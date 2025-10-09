using Evaluation.DAL.Entities.AdminPanel;

namespace Evaluation.DAL.Entities.OrganizationTree;

public class SchoolLevel : BaseEntities
{
    public int SchoolId { get; set; }
    public School School { get; set; } = null!;
    public int LevelId { get; set; }
    public Level Level { get; set; } = null!;
}
