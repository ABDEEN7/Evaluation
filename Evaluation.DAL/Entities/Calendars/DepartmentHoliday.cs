using Evaluation.DAL.Entities.BaseModule;

namespace Evaluation.DAL.Entities.Calendars;

public class DepartmentHoliday : EntityBase 
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsCronExpression { get; set; }
    public string CronExpression { get; set; } = string.Empty;
    public Guid AcademicYearId { get; set; }
    public AcademicYear AcademicYear { get; set; } = null!;
}
