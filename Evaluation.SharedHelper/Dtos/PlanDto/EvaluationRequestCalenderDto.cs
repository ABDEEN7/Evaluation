
namespace Evaluation.SharedHelper.Dtos.PlanDto;

public class EvaluationRequestCalenderDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Start { get; set; }
    public string End { get; set; }
    public string? Color { get; set; } = "#8a1538";
    public string? Source { get; set; }
}