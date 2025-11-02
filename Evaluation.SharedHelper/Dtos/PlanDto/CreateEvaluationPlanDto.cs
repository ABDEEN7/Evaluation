using Evaluation.SharedHelper.Dtos.SchoolDto;

namespace Evaluation.SharedHelper.Dtos.PlanDto;

public class CreateEvaluationPlanDto
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid PlayType { get; set; }
    public List<SelectedSchool>? Schools { get; set; }
    public Guid AcademicYearId { get; set; }
    public Guid PlanStatusId { get; set; }
}
