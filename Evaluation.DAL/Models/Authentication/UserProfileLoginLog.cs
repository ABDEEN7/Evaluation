using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.Authentication
{
    public class UserProfileLoginLog : EntityBase, IAuditLogEntity
    {
        public string IP { get; set; } = null!;
        public string? UserAgent { get; set; }
        public Guid UserProfileId { get; set; }
        public UserProfile? UserProfile { get; set; }
    }
}
