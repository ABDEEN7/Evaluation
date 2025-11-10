

using Scholarship.SharedHelper.Models.Admin;



namespace Evaluation.SharedHelper.Models.Api.FormBuilderDTO
{
    public class SystemFieldDTO : EntityBaseDTO
    {
        public string TitleAr { get; set; } = null!;
        public string TitleEn { get; set; } = null!;
        public string BackendName { get; set; } = null!;
        public Guid FieldTypeId { get; set; }
        public string FieldType { get; set; } = null!;
        public Guid SystemModuleId { get; set; }
        public string SystemModule { get; set; } = null!;
        public bool IsCoreColumn { get; set; }
        public bool IsRequired { get; set; }
        public Guid SystemTableId { get; set; }
        public string? SystemTable { get; set; } = null;
        public Guid? RelatedFieldsGroupId { get; set; }
        public string? RelatedFieldsGroup { get; set; }
        public Guid? DropDownTypeId { get; set; }
        public string? DropDownType { get; set; }
        public int? OrderNo { get; set; }
        public int? IsReadOnly { get; set; }
    }
}
