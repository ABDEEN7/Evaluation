using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.Org
{
    public class SchoolGradeSction : EntityBase, IAuditLogEntity
    {
        public Guid SchoolLevelId { get; set; }
        public SchoolLevel? SchoolLevel { get; set; }
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string Grade { get; set; } = null!;
        public string? Type { get; set; }
        public string? IntegrationCode { get; set; }
       
    }
}
