using Evaluation.DAL.Entities.BaseModule;

namespace Evaluation.DAL.Entities.Planing.TeamsModule;

public class UserTeam : BaseEntities
{
    public Guid TeamId { get; set; }
    public Guid UserId { get; set; }
}
