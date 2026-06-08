using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.Org
{
    public class SchoolGradeSectionCourse : EntityBase, IAuditLogEntity
    {
        public Guid SchoolGradeSctionId { get; set; }
        public SchoolGradeSection? SchoolGradeSction { get; set; }
        public Guid SchoolCourseId { get; set; }
        public SchoolCourse? SchoolCourse { get; set; }
        public int Duration { get; set; } = 0;
        public string QID { get; set; }

        public Guid? SchoolTermId { get; set; }
        public SchoolTerm? SchoolTerm { get; set; }
    }
}
