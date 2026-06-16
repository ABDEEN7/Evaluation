using Evaluation.SharedHelper.Dtos.EvalFormDto;

namespace Evaluation.DAL.Dtos.Form;

public class FormItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public List<SubFormItemDto>? SubFormItems { get; set; }
    public int OrderNo { get; set; } = 0;
    public bool HasNote { get; set; }
    public bool NoteRequired { get; set; }

    public List<RelatedItemDto>? RelatedItems { get; set; }
    public List<FormItemConfigDto>? FormItemConfigs { get; set; }
    public bool HasMultiEvaluation { get; set; }
    public decimal? Value { get; set; }

    //public Guid RelatedItemId { get; set; } 
    //public string RelatedItemName { get; set; } = null!;

}

public class RelatedItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Value { get; set; } = null!;
    public string Note { get; set; } = null!;
}