

namespace Evaluation.SharedHelper.Models.Admin
{
    public class FieldDropDownValueDTO : EntityBaseDTO
    {

        public string TitleEn { get; set; } = null!;
        public string TitleAr { get; set; } = null!;
        public string? DropDownBackendName { get; set; }
        public Guid DropDownTypeId { get; set; }
        public string dropDownType { get; set; } = null!;
        public Guid? ParentDropDownId { get; set; }
        public string? ParentDropDown { get; set; }
        public int? OrderNo { get; set; }
    }
}
