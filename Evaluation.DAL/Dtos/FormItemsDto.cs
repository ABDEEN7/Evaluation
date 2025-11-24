using Evaluation.DAL.Models.FormsModules;

namespace Evaluation.DAL.Dtos;

public class FormItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public List<SubFormItemDto>? SubFormItems { get; set; }
}