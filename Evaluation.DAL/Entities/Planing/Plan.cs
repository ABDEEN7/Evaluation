using Evaluation.DAL.Entities.AdminPanel;
using Evaluation.DAL.Entities.Calendars;

namespace Evaluation.DAL.Entities.Planing;

public class Plan : BaseEntities
{
    public required string NameAr { get; set; }
    public required string NameEn { get; set; }
    public int DepartmentId { get; set; }
    public int AcademicYearId { get; set; }
    public Department? Department { get; set; }
    public AcademicYear? AcademicYear { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string? ExpectedListJson { get; set; }
    public int PlanStatusId { get; set; }
    public int PlanScheduleId { get; set; }
    public PlanStatus? PlanStatus { get; set; }
    public PlanSchedule? PlanSchedule { get; set; }
    public ICollection<ChangeRequest>? ChangeRequests { get; set; }

}
