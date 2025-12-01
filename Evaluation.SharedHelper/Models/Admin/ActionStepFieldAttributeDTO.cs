namespace Evaluation.SharedHelper.Models.Admin
{
    public class ActionFieldAttributeDTO : EntityBaseDTO
    {
        public Guid FieldId { get; set; }
        public string AttributeKey { get; set; } = null!;
        public string? AttributeValue { get; set; }
        public string? MessageAr { get; set; }
        public string? MessageEn { get; set; }
        public string Description { get; set; } = null!;
        public Guid? ActionFieldId { get; set; }
    }
}
