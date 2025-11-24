
namespace Evaluation.DAL.Dtos.Form;

public class FormEvaluationDto
{
    public Guid Id { get; set; }
    public List<FormItemEvaluationDto>? Items { get; set; }
    public string? Strengths { get; set; }
    public string? Improvements { get; set; }
}
