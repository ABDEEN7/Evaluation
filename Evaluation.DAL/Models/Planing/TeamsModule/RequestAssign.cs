using Evaluation.DAL.Models.BaseModule;

namespace Evaluation.DAL.Models.Planing.TeamsModule;

public class RequestAssign: EntityBase
{
    public Guid RequestId { get; set; }
    public Guid UserId { get; set; }
    public bool IsLeader { get; set; }
}
