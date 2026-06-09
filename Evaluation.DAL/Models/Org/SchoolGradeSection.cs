using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.Org
{
    public class SchoolGradeSection : EntityBase, IAuditLogEntity
    {
        public Guid SchoolGradeId { get; set; }
        public SchoolGrade? SchoolGrade { get; set; }

        public string SectionAr { get; set; } = null!;
        public string SectionEn { get; set; } = null!;
        public string? IntegrationCode { get; set; }
    }
}
