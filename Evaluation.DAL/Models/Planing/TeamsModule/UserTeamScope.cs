using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Models.UserEntiy;

namespace Evaluation.DAL.Models.Planing.TeamsModule;

public class UserTeamScope : EntityBase
{
    public Guid UserTeamId { get; set; }
    public UserTeam? UserTeam { get; set; }
    public Guid ScopeId { get; set; }
    public Scope? Scope { get; set; }
}
