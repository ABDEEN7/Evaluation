namespace Evaluation.SharedHelper.Models.Api.FormBuilderDTO
{
    public class DropDownValueDTO : EntityBaseDTO
    {
        public string TitleEn { get; set; } = null!;
        public string TitleAr { get; set; } = null!;
        public int? OrderNo { get; set; }
        public Guid DropDownTypeId { get; set; }
        public Guid? ParentDropDownId { get; set; }
        public decimal? GPA { get; set; }
        public int? TotalHours { get; set; }
    }
}
