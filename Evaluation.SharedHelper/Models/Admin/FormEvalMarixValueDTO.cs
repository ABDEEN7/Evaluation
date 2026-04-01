


namespace Evaluation.SharedHelper.Models.Admin
{
    public class FormEvalMarixValueDTO : EntityBaseDTO
    {
        public Guid FormEvalMatrixId { get; set; }
        public string? FormEvalMatrix { get; set; }
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public decimal MinValue { get; set; }
        public decimal MaxValue { get; set; }
        public decimal ActualMatrixValue { get; set; }
        public string? DescAr { get; set; }
        public string? DescEn { get; set; }
        public int OrderNo { get; set; } = 0;
        public int NextEvalDays { get; set; } = 0;

    }
}
