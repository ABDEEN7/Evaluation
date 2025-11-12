

using Evaluation.SharedHelper.Models.Api.ServiceDTOs;

namespace Evaluation.SharedHelper.Models.Api.FormBuilderDTO
{
    public class FieldDTO: EntityBaseDTO
    {
        public string Label { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
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
        public Guid[] FieldPartyTypes { get; set; }
        public Guid? FormGroupListId { get; set; }
        public FormGroupDTO? FormGroupList { get; set; }

        public string? ClassName { get; set; }
        public Guid? MappingSystemFieldId { get; set; }
        public SystemFieldDTO? MappingSystemField { get; set; }
        public Guid? ReadSystemFieldId { get; set; }
        public SystemFieldDTO? ReadSystemField { get; set; }
        public Guid? DropDownParentFieldId { get; set; }
        public FieldDTO? DropDownParentField { get; set; }
        public Guid? DropDownTypeId { get; set; }
        public DropDownTypeDTO? DropDownType { get; set; }
        public Guid? FormGroupCustomListId { get; set; }
        public FormGroupCustomListDTO? FormGroupCustomList { get; set; }
        public List<AttributeDTO> Attributes { get; set; }
        public string? FormGroupName { get; set; }
    }
}
