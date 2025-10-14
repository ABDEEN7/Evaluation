using Evaluation.DAL.Entities.BaseModule;

namespace Evaluation.DAL.Entities.Planing.TeamsModule;

public class UserTeam : EntityBase
{
    public Guid TeamId { get; set; }
    public Guid UserId { get; set; }
}
