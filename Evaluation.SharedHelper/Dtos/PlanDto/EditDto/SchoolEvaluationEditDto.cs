namespace Evaluation.SharedHelper.Dtos.PlanDto.EditDto;

public class SchoolEvaluationEditDto
{
    public Guid SchoolId { get; set; }
    public string SchoolName { get; set; } = null!;

    public bool IsSelected { get; set; }

    // Only meaningful if IsSelected = true
    public Guid? DepEvaluationTypeId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
