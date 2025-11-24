namespace Evaluation.DAL.Dtos;

public class FormItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public List<SubFormItemDto>? SubFormItems { get; set; }
    public string OrderNo { get; set; } = null!;
    public bool HasNote { get; set; }
}