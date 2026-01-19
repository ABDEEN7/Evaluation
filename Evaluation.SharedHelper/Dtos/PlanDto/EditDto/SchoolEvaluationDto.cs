namespace Evaluation.SharedHelper.Dtos.PlanDto.EditDto;

public class SchoolEvaluationDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public Guid? VisitTypeId { get; set; }
    public bool HasEvaluationRequest { get; set; }
    public Guid? EvaluationRequestId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
