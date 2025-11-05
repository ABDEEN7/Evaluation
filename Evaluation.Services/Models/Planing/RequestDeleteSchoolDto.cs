namespace Evaluation.Services.Models.Planing;

public class RequestDeleteSchoolDto
{
    public Guid SchoolId { get; set; }
    public Guid EvaluationId { get; set; }
    public string Reason { get; set; } = null!;
}
