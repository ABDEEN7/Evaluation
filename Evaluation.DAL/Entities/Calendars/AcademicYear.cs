using Evaluation.DAL.Entities.AdminPanel;
using Evaluation.DAL.Entities.Planing;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.DAL.Entities.Calendars;
[Index(nameof(Department), nameof(Year), IsUnique = true)]

public class AcademicYear : BaseEntities
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Year { get; set; }
    public Guid DepartmentId { get; set; }
    public Department Department { get; set; } = new();
}