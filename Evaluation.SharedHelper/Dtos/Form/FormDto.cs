using Evaluation.DAL.Dtos.Form;
using Evaluation.SharedHelper.Dtos.EvalFormDto;

namespace Evaluation.SharedHelper.Dtos.Form;

public class FormDto
{
    public EvaluationFormDto EvalForm { get; set; }
    public List<FormItemDto> Items { get; set; }
}
