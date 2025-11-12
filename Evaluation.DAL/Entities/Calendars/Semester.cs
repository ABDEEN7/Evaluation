using Evaluation.DAL.Entities.BaseModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Entities.Calendars
{
    public class Semester : EntityBase
    {
        public Guid AcademicYearId { get; set; }
        public AcademicYear? AcademicYear { get; set; }
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
