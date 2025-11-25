using Evaluation.DAL.Models.FormsModules;

namespace Evaluation.DAL.Dtos;

public class FormEvaluationValueDto
{
    public Guid Id { get; set; }
    public List<FormItemValue>? Items { get; set; }
    public List<SubFormItemValue>? SubItems { get; set; }
}
