using Evaluation.DAL.Models.Audit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.Base
{
    [Table("MinistryUser")]
    public class MinistryUser : UserProfile, IAuditLogEntity, ILocalizedFull
    {
        public string? JobDescription { get; set; }
        public string? Avatar { get; set; }
        public string? EmpNo { get; set; }


    }
}
