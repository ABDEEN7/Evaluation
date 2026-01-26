using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;

namespace Evaluation.DAL.Models.Calendars;

public class DepartmentHoliday : EntityBase , IAuditLogEntity
{
    public Guid AcademicYearId { get; set; }
    public AcademicYear? AcademicYear { get; set; } 
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsCronExpression { get; set; }
    public string? CronExpression { get; set; } 
    
}
