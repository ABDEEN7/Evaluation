

namespace Evaluation.SharedHelper.Models.Admin
{
    public class PlanTypeDepDTO : EntityBaseDTO
    {
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public Guid DepartmentId { get; set; }
        public string? Department { get; set; }
        public Guid PlanTypeId { get; set; }
        public string? PlanType { get; set; }
        public int OrderNo { get; set; } = 0;

    }
}
