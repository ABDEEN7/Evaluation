using  Evaluation.DAL.Entities.BaseModule;

namespace Evaluation.DAL.Entities.Planing.TeamsModule;

public class RequestAssign: EntityBase
{
    public Guid RequestId { get; set; }
    public Guid UserId { get; set; }
    public bool IsLeader { get; set; }
}
