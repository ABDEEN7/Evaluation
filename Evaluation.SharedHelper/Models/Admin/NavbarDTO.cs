

namespace Evaluation.SharedHelper.Models.Admin
{
    public class NavbarDTO : EntityBaseDTO
    {

        public string TitleAr { get; set; } = null!;

        public string TitleEn { get; set; } = null!;


        public Guid? ParentId { get; set; }
        public string? Parent { get; set; }

        public bool IsInternal { get; set; }

        public string UrlAr { get; set; } = null!;

        public string UrlEn { get; set; } = null!;

        public int? OrderNo { get; set; }

        public string? Target { get; set; }
        public bool IsAuthorized { get; set; }
        public string? PermissionId { get; set; }
        public string? Permission { get; set; }
        public int? Level { get; set; }
        public int? LevelsUp { get; set; }
        public int? LevelsDown { get; set; }
        public Guid? DepartmentId { get; set; }

    }
}

