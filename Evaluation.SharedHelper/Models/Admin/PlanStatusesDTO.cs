

namespace Evaluation.SharedHelper.Models.Admin
{
    public class PlanStatusesDTO : EntityBaseDTO
    {
        public string NameAr { get; set; } = null!;
        public string NameEN { get; set; } = null!;
        public string BackendName { get; set; } = null!;
        public int OrderNo { get; set; }

    }
}
