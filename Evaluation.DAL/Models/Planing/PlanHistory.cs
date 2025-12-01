using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.Calendars;
using Evaluation.DAL.Models.DepartementEntites;

namespace Evaluation.DAL.Models.Planing;

public class PlanHistory : EntityBase
{
	public Guid PlanId { get; set; }
	public Plan? Plan { get; set; }
	public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string? ExpectedListJson { get; set; }
    public Guid DepartmentId { get; set; }
    public Department? Department { get; set; }
    public Guid AcademicYearId { get; set; }
    public AcademicYear? AcademicYear { get; set; }
    public Guid PlanStatusId { get; set; }
    public PlanStatus? PlanStatus { get; set; }
    public Guid PlanTypeId { get; set; }
    public PlanType? PlanType { get; set; }

}
