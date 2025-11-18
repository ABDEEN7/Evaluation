using Evaluation.DAL.Entities.Calendars;
using Evaluation.DAL.Entities.DepartementEntites;
using Evaluation.DAL.Entities.Planing;

namespace Evaluation.SharedHelper.Dtos.PlanDto;

public class UpdatePlanDto
{
    public string Name { get; set; } = null!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public Guid DepartmentId { get; set; }
    public Guid AcademicYearId { get; set; }
    public Guid PlanStatusId { get; set; }
    public Guid PlanTypeDepartmentId { get; set; }
    public Guid? SemesterId { get; set; }
    public string? PlanJsonValue { get; set; }

}
