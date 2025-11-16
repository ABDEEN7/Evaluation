using Microsoft.EntityFrameworkCore;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.DepartementEntites;

namespace Evaluation.DAL.Models.Calendars;
[Index(nameof(DepartmentId), nameof(Year), IsUnique = true)]

public class AcademicYear : EntityBase
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Year { get; set; }
    public Guid DepartmentId { get; set; }
    public Department? Department { get; set; }
	public bool IsCurrent { get; set; }
}