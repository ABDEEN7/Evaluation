

namespace Evaluation.SharedHelper.Models.Admin
{
    public class DropDownTypeDTO : EntityBaseDTO
    {
        public string BackendName { get; set; } = null!;
        public string TitleAr { get; set; } = null!;
        public string TitleEn { get; set; } = null!;
        public string? DataSourceTable { get; set; }
        public Guid? ParentId { get; set; }
        public string? Parent { get; set; }
    }
}
