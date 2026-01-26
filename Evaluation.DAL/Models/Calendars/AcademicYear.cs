using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.DepartementEntites;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.DAL.Models.Calendars;
[Index(nameof(DepartmentId), nameof(Year), IsUnique = true)]

public class AcademicYear : EntityBase, IAuditLogEntity
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int Year { get; set; }
    public Guid DepartmentId { get; set; }
    public Department? Department { get; set; }
	public bool IsCurrent { get; set; }
}