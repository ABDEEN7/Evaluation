

namespace Evaluation.SharedHelper.Models.Admin
{
    public class EvaluationTypeDTO : EntityBaseDTO
    {
        public string BackendName { get; set; } = null!;
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public int OrderNo { get; set; }

    }
}
