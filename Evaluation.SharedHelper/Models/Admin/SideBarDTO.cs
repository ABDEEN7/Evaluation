namespace Evaluation.SharedHelper.Models.Admin
{
    public class SideBarDTO : EntityBaseDTO
    {
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public Guid? PermissionId { get; set; } = null;
        public string? Permission { get; set; } = null;
        public Guid? ParentId { get; set; }
        public string? Parent { get; set; }
        public string? Icon { get; set; }
        public string RoutingPath { get; set; } = null!;
        public int OrderNo { get; set; }
        public ICollection<SideBarDTO>? ChildList { get; set; }
        public string Name { get; set; } = null!;

    }
}
