using Evaluation.DAL.Models.FormBuilder;

namespace Evaluation.Services.BusinessLayer.API.Template;

public class PlaceholderDto
{
    public string? Key { get; set; }
    public string? Value { get; set; }
    public PlaceholderType PlaceholderType { get; set; } 
    public Guid? FieldListId { get; set; }
    public string? ChildFieldId { get; set; }
    public Field? ChildField { get; set; }
}