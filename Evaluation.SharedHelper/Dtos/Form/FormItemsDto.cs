namespace Evaluation.DAL.Dtos.Form;

public class FormItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public List<SubFormItemDto>? SubFormItems { get; set; }
    public int OrderNo { get; set; } = 0;
    public bool HasNote { get; set; }

    public Guid RelatedItemId { get; set; } 
    public string RelatedItemName { get; set; } = null!;
    public string RelatedItemValue { get; set; } = null!;
    public string RelatedItemNote { get; set; } = null!;

}