using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.UserEntiy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.Authentication
{
    public class UserLoginLog : EntityBase, IAuditLogEntity
    {
        public string IP { get; set; } = null!;
        public string? UserAgent { get; set; }
        public Guid UserId { get; set; }
        public MinistryUser? User { get; set; }
    }
}
