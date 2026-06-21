using Evaluation.DAL.Dtos.Form;
using Evaluation.DAL.Helper;
using Evaluation.SharedHelper.Dtos.EvalFormDto;

namespace Evaluation.SharedHelper.Dtos.Form;

public class FormDto
{
    public TemplateFormDto EvalForm { get; set; }
    public List<ScopeTreeDto> Tree { get; set; }
    //public List<FormItemDto> Items { get; set; }
}
