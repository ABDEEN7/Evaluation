using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.Org
{
    public class GradeLevel : EntityBase, IAuditLogEntity
    {
        public Guid EducationLevelId { get; set; }
        public EducationLevel? EducationLevel { get; set; } = null!;

        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string Grade { get; set; } = null!;
        public int orderNo { get; set; } = 0;
        public string? IntegrationCode { get; set; }

    }
}
