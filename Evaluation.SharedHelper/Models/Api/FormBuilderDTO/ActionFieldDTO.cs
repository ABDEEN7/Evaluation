

namespace Evaluation.SharedHelper.Models.Api.FormBuilderDTO
{
    public class ActionFieldDTO:EntityBaseDTO
    {
        public string FieldId { get; set; } = null!;
        public string Field { get; set; } = null!;
        public string ServiceActionId { get; set; } = null!;
        public string ServiceAction { get; set; } = null!;
        public bool IsEditable { get; set; }
        public int? OrderNo { get; set; }
    }
}
