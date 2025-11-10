

namespace Evaluation.SharedHelper.Models.Api.FormBuilderDTO
{
    public class FieldAttributeValueDTO : EntityBaseDTO
    {
        public Guid FieldId { get; set; }
        public FieldDTO Field { get; set; }
        public string AttributeKey { get; set; } = null!;
        public string? AttributeValue { get; set; }
        public string? MessageAr { get; set; }
        public string? MessageEn { get; set; }
        public string Description { get; set; } = null!;

    }
}
