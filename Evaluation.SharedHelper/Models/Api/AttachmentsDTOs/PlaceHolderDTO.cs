

namespace Evaluation.SharedHelper.Models.Api.AttachmentsDTOs
{
    public class PlaceHolderDTO : EntityBaseDTO
    {
        public Guid ServiceId { get; set; }
        public string Service { get; set; } = null!;
        public string? PlaceHolderName { get; set; }
        public string? TypeDisplay { get; set; }
        public string?  FieldId { get; set; }
        public string? Field { get; set; }
        public string[]? ChildFieldIds { get; set; }
        public string? Type { get; set; }
        public string? ColumnName { get; set; }
    }
}
