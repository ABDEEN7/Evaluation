using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.UserEntiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.Planing.TeamsModule
{
    public class UserTeam : EntityBase
    {
        public Guid TeamId { get; set; }
        public Team? Team { get; set; }
        public Guid UserId { get; set; }
        public MinistryUser? User { get; set; }
        public ICollection<UserTeamScope> UserTeamScope { get; set; }
    }
}
