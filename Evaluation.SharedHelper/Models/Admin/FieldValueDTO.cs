

namespace Evaluation.SharedHelper.Models.Admin
{
    public class FieldValueDTO
    {

        public Guid? FieldId { get; set; }
        public string? Value { get; set; }
        public string? FieldName { get; set; }
        public string? FieldTooltip { get; set; }
        public int? Row { get; set; }
        public int? Column { get; set; }
        public string? Type { get; set; }
        public Guid? FormGroupId { get; set; }
        public Guid? FormGroupListId { get; set; }
        public string? FormGroupName { get; set; }
        public int FormGroupOrderNo { get; set; } = 9999;
        public string? BackendName { get; set; }
        public string? ClassName { get; set; }
        public Guid? DropDownTypeId { get; set; }
        public Guid? DropDownParentFieldId { get; set; }
        public Guid? ReadFromFieldId { get; set; }
        public string? Label { get; set; }
        public string? FieldTypeInfo { get; set; }
        public bool?  IsApproved { get; set; }
        public bool? IsEditable { get; set; }
        public bool? Visible { get; set; }
       
        public string? ReadFromColumnName { get; set; }
        public string? JsonSchema { get; set; }

        public IList<FieldViewConditionDTO?>? Conditions { get; set; }
        public IList<AttributeDTO?>? Attributes { get; set; }

    }
}
