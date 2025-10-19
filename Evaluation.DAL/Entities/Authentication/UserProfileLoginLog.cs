using Evaluation.DAL.Entities.Audit;
using  Evaluation.DAL.Entities.BaseModule;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Entities.Authentication
{
    public class UserLoginLog : EntityBase, IAuditLogEntity
    {
        public string IP { get; set; } = null!;
        public string? UserAgent { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
    }
}
