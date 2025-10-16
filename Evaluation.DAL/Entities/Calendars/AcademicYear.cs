using Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Planing;
using Microsoft.EntityFrameworkCore;
using Evaluation.DAL.Entities.Base;

namespace Evaluation.DAL.Entities.Calendars;
[Index(nameof(Department), nameof(Year), IsUnique = true)]

public class AcademicYear : EntityBase
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Year { get; set; }
    public Guid DepartmentId { get; set; }
    public Department Department { get; set; } = new();
}