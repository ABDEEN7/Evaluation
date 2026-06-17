using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.Org
{
    public class SchoolGrade : EntityBase, IAuditLogEntity
    {
        public Guid SchoolLevelId { get; set; }
        public SchoolLevel? SchoolLevel { get; set; }
        public Guid GradeLevelId { get; set; }
        public GradeLevel? GradeLevel { get; set; }
        public string? Type { get; set; }
       
    }
}
