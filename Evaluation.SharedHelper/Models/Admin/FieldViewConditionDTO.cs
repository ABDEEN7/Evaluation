
namespace Evaluation.SharedHelper.Models.Admin
{
    public class FieldViewConditionDTO : EntityBaseDTO
    {
        
        public Guid? FieldId { get; set; }

        public FieldDTO Field { get; set; } = new();
        public string operators { get; set; } = null!;
        public string FieldValue { get; set; } = null!;
        public bool IsSufficient { get; set; }

        public string[] FieldDropDownValueIds { get; set; } = [];
        public Guid? ParentFieldId { get; set; }
        public string? ParentField { get; set; }
        public string FieldValueDisplay { get; set; } = null!;
    }
}
