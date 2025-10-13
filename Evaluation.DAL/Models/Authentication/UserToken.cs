using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.Authentication
{
    public class UserToken : EntityBase, IAuditLogEntity
    {
        public string Token { get; set; } = null!;
        public bool Deprecated { get; set; }
        public DateTime TokenExpiryDate { get; set; }
        public DateTime? DeprecatedDate { get; set; }
        public string? UserAgent { get; set; }
        public string? IP { get; set; }
        public Guid UserProfileId { get; set; }
        public UserProfile? UserProfile { get; set; }
    }
}
