using Evaluation.SharedHelper.Dtos.PlanDto;

namespace Evaluation.SharedHelper.Dtos.SchoolDto;

public class SelectedSchool : BaseDto
{
    public DateTime StartEvaluationDate { get; set; }
    public DateTime EndEvaluationDate { get; set; }
    public Guid VisitTypeId { get; set; }
    public string? Name { get; set; }
    public string Rating { get; set; }
}