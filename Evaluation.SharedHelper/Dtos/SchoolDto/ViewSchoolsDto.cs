using Evaluation.SharedHelper.Dtos.PlanDto;

namespace Evaluation.SharedHelper.Dtos.SchoolDto;

public class ViewSchoolsDto : BaseDto
{
    public DateTime StartEvaluationDate { get; set; }
    public DateTime EndEvaluationDate { get; set; }
    public Guid VisitTypeId { get; set; }
    public string? Name { get; set; }
    public string Rating { get; set; }
    public bool IsSelected { get; set; } = true;
}