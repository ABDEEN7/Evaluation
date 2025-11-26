
namespace Evaluation.DAL.Dtos.Form;

public class FormItemEvaluationDto
{
    public Guid Id { get; set; }
    public Guid ValueId { get; set; }
    public decimal Value { get; set; }
    public string? Note { get; set; }
    public List<SubFormItemEvaluationDto>? SubItems { get; set; }
}