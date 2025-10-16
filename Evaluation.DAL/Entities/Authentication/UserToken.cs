using Evaluation.DAL.Entities.Audit;
using Evaluation.DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Entities.Authentication
{
    public class UserToken : BaseEntities, IAuditLogEntity
    {
        public string Token { get; set; } = null!;
        public bool Deprecated { get; set; }
        public DateTime TokenExpiryDate { get; set; }
        public DateTime? DeprecatedDate { get; set; }
        public string? UserAgent { get; set; }
        public string? IP { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
    }
}
