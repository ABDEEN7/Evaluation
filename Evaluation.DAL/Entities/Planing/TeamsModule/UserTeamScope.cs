using Evaluation.DAL.Entities.Authentication;
using  Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.FormsModules;

namespace Evaluation.DAL.Entities.Planing.TeamsModule;

public class UserTeamScope : EntityBase
{
    public Guid TeamId { get; set; }
    public Team? Team { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public Guid ScopeId { get; set; }
    public Scope? Scope { get; set; }
}
