using Evaluation.DAL.Entities.AdminPanel;
using Evaluation.DAL.Entities.Planing;

namespace Evaluation.DAL.Entities.Calendars;

public class DepartmentHoliday : BaseEntities // i change DepartmentDayOff to DepartmentHolidays
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsCronExpression { get; set; }
    public string CronExpression { get; set; } = string.Empty;
    public Guid DepartmentId { get; set; }
    public Guid AcademicYearId { get; set; }
    public Department Department { get; set; } = null!;
    public AcademicYear AcademicYear { get; set; } = null!;
}
