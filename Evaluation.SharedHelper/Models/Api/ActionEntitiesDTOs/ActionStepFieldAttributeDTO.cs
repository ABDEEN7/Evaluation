namespace Evaluation.SharedHelper.Models.Api.ActionEntitiesDTOs
{
    public class ActionStepFieldAttributeDTO : EntityBaseDTO
    {
        public Guid FieldId { get; set; }
        public string AttributeKey { get; set; } = null!;
        public string? AttributeValue { get; set; }
        public string? MessageAr { get; set; }
        public string? MessageEn { get; set; }
        public string Description { get; set; } = null!;
        public Guid? ActionStepFieldId { get; set; }
    }
}
