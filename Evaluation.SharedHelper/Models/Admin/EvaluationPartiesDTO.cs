

namespace Evaluation.SharedHelper.Models.Admin
{
    public class EvaluationPartiesDTO : EntityBaseDTO
    {
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public int OrderNo { get; set; }
        public Guid DepartmentId { get; set; }
        public string? Department { get; set; }

    }
}
