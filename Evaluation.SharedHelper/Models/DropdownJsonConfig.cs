

namespace Evaluation.SharedHelper.Models
{
    public class DropdownJsonConfig
    {
        public string IdName { get; set; } = null!; // Default is Id
        public string DisplayNameAr { get; set; } = null!;// ex. NameAr
        public string DisplayNameEn { get; set; } = null!; // ex. NameEn
        public string TableNameSource { get; set; } = null!; // table name Service
        public string ParentControlName { get; set; } = null!;// control name of the parent dropdown in case existing
        public string ParentReferenceId { get; set; } = null!; // DepartmentId
        public string? IsActive { get; set; } // false , true or null
        public Select2Config Select2Config { get; set; } = new(); // Select2Config
    }


    public class Select2Config
    {
        public string width { get; set; } = null!;
        public bool allowClear { get; set; }
        public bool multiple { get; set; }
        public string dropdownCssClass { get; set; } = null!;
    }
}
