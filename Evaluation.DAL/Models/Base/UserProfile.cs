using Evaluation.DAL.Models.Audit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.Base
{
    public class UserProfile : EntityBase, ILocalizedFull, IAuditLogEntity
    {
        public string QID { get; set; } = null!;
        public bool IsActivated { get; set; }
        public string Type { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? NationalityCode { get; set; }
        public string FullNameAr { get; set; } = null!;
        public string FullNameEn { get; set; } = null!;
        public DateTime? LastLoginDate { get; set; }
        public string PreferredLanguage { get; set; } = null!;
        public string? Mobile { get; set; }

      

    }
}
