using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.UserEntiy;

namespace Evaluation.DAL.Models.Planing.TeamsModule
{
    public class UserTeam : EntityBase, IAuditLogEntity
    {
        public Guid TeamId { get; set; }
        public Team? Team { get; set; }
        public Guid UserId { get; set; }
        public MinistryUser? User { get; set; }
    }
}
