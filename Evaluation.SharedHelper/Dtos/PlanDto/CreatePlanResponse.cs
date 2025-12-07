using Evaluation.SharedHelper.Dtos.SchoolDto;

namespace Evaluation.SharedHelper.Dtos.PlanDto;

public class CreatePlanResponse : BaseDto
{
    public string Name { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public Guid PlanTypeId { get; set; }
    public List<SelectedSchool>? Schools { get; set; }
    public Guid? AcademicYearId { get; set; }
    public Guid PlanStatusId { get; set; }
    public Guid? SemesterId { get; set; }
}
