using Evaluation.DAL.Models.FormBuilder;
using Evaluation.SharedHelper.Dtos.Form;

namespace Evaluation.Services.BusinessLayer.API.Template;

public class PlaceholderDto
{
    public string? Key { get; set; }
    public string? Value { get; set; }
    public PlaceholderType PlaceholderType { get; set; } 
    public Guid? FieldListId { get; set; }
    public string? ChildFieldId { get; set; }
    public Field? ChildField { get; set; }
    public List<SchoolPerformanceResult>? SchoolPerformanceResult { get; set; }
    public List<ScopeTreeDto>? Tree { get; set; }
}