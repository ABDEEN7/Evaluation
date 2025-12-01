

namespace Evaluation.SharedHelper.Models.Admin
{
    public class FieldDTO: EntityBaseDTO
    {
        public string Label { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string TitleAr { get; set; } = null!;
        public string TitleEn { get; set; } = null!;
        public string BackendName { get; set; } = null!;
        public Guid ServiceId { get; set; }
        public ServiceDTO Service { get; set; } = null!;
        public string? InfoAr { get; set; }
        public string? InfoEn { get; set; }
        public Guid FieldTypeId { get; set; }
        public string FieldType { get; set; } = null!;
        public string? Description { get; set; }
        public int Column { get; set; }
        public int Row { get; set; }
        public int? FieldCount { get; set; }
        public Guid FormGroupId { get; set; }
        public FormGroupDTO FormGroup { get; set; } = null!;
        public Guid[] FieldPartyTypes { get; set; } = new Guid[0];
        public Guid? FormGroupListId { get; set; }
        public FormGroupDTO? FormGroupList { get; set; }

        public string? ClassName { get; set; }
        public Guid? MappingFieldId { get; set; }
        public FieldDTO? MappingField { get; set; }
        public Guid? ReadFieldId { get; set; }
        public FieldDTO? ReadField { get; set; }
        public Guid? DropDownParentFieldId { get; set; }
        public FieldDTO? DropDownParentField { get; set; }
        public Guid? DropDownTypeId { get; set; }
        public DropDownTypeDTO? DropDownType { get; set; }
        public Guid? FormGroupCustomListId { get; set; }
        public FormGroupCustomListDTO? FormGroupCustomList { get; set; }
        public List<AttributeDTO> Attributes { get; set; } = new();
        public string? FormGroupName { get; set; }
    }
}
