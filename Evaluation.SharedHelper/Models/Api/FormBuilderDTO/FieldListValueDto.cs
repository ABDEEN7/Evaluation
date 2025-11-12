namespace Evaluation.SharedHelper.Models.Api.FormBuilderDTO
{
    public class FieldListValueDto
    {
        public Guid? SystemFieldId { get; set; } 
        public string? FieldBackendName { get; set; } 
        public string Value { get; set; }           
    }

    public class ExtractedObjectDto
    {
        public List<FieldListValueDto>? Fields { get; set; } = new List<FieldListValueDto>(); 
    }
}
