using Evaluation.DAL.Models.BaseModule;

namespace Evaluation.DAL.Models.Calendars
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
