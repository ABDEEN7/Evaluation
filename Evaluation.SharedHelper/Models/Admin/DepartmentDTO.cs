

namespace Evaluation.SharedHelper.Models.Admin
{
    public class DepartmentDTO : EntityBaseDTO
    {
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string RoutingPath { get; set; } = null!;
        public string BackendName { get; set; } = null!;
        public string DepIcon { get; set; } = null!;
        public Guid? TargetOrgTreeId { get; set; }
        public string? TargetOrgTree { get; set; }
        public string? Category { get; set; }
        public Guid CategoryId { get; set; }
        public bool IsNDA { get; set; }
        public string? DescAr { get; set; }
        public string? DescEn { get; set; }
        public int OrderNo { get; set; } = 0;

    }
}
