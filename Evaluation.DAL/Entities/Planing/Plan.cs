using Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Calendars;
using Evaluation.DAL.Entities.DepartementEntites;

namespace Evaluation.DAL.Entities.Planing;

public class Plan : EntityBase
{
    public string PlanName { get; set; } = null!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public Guid DepartmentId { get; set; }
    public Department? Department { get; set; }
    public Guid AcademicYearId { get; set; }
    public AcademicYear? AcademicYear { get; set; }
    public Guid PlanStatusId { get; set; }
    public PlanStatus? PlanStatus { get; set; }
    public Guid PlanTypeDepartmentId { get; set; }
    public PlanTypeDepartment   ? PlanTypeDepartment { get; set; }
    public Guid? SemesterId { get; set; }
    public Semester? Semester { get; set; }
    public string? PlanJsonValue { get; set; }

}
