using  Evaluation.DAL.Entities.BaseModule;

namespace Evaluation.DAL.Entities.Org;

public class SchoolLevel : EntityBase
{
    public Guid SchoolId { get; set; }
    public School School { get; set; } = null!;
    //this table to make many to many relation-ship becuase every scholl have multuple level and every level have multiple school
    public Guid LevelId { get; set; }
    public Level Level { get; set; } = null!;
}
