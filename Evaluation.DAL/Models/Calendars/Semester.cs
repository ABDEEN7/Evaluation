using Evaluation.DAL.Models.BaseModule;

namespace Evaluation.DAL.Models.Calendars
{
    public class Semester : EntityBase
    {
        public string BackendName { get; set; } = null!;

        public string? NSISCode { get; set; }
        public Guid AcademicYearId { get; set; }
        public AcademicYear? AcademicYear { get; set; }
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int OrderNo { get; set; } = 0;

    }
}
