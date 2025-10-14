using Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Authentication;

namespace Evaluation.DAL.Entities.Planing.TeamsModule;

public class ScopeUserTeam : EntityBase
{
    public Guid UserId { get; set; }
    public Guid TeamId { get; set; }
    public Guid EvaluationFieldId { get; set; }
    public User? User { get; set; }
    public Team? Team { get; set; }
}