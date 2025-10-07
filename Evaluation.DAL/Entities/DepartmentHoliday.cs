using Evaluation.DAL.Entities.AdminPanel;

namespace Evaluation.DAL.Entities;

public class DepartmentHoliday : BaseEntities // i change DepartmentDayOff to DepartmentHolidays
{
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string CronExpression { get; set; }
    public Guid DepartmentId { get; set; }
    public Guid AcademicYearId { get; set; }
    public Department Department { get; set; }
    public AcademicYear AcademicYear { get; set; }
}
