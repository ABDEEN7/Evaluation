namespace Evaluation.SharedHelper.Models.Admin
{
	public class ActionConditionDTO:EntityBaseDTO
	{
		public Guid ServiceActionId { get; set; }
        public string Type { get; set; } = null!;
        public Guid RefID { get; set; }
        public string Ref { get; set; } = null!;
        public string operators { get; set; } = null!;
        public string FieldValue { get; set; } = null!;
        public string FieldValueDisplay { get; set; } = null!;
        public string[] FieldDropDownValueIds { get; set; } = null!;
    }
}
