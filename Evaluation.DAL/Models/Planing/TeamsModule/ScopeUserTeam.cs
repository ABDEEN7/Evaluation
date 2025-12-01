using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Models.UserEntiy;

namespace Evaluation.DAL.Models.Planing.TeamsModule;

public class ScopeUserTeam : EntityBase
{
    public Guid UserId { get; set; }
    public MinistryUser User { get; set; } = null!;
    public Guid ScopeId { get; set; }
    public Scope Scope { get; set; } = null!;
    public Guid TeamId { get; set; }
    public Team Team { get; set; } = null!;
}