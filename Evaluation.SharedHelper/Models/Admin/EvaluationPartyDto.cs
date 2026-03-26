namespace Evaluation.SharedHelper.Models.Admin;

public class EvaluationPartyDto : EntityBaseDTO
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public int OrderNo { get; set; } = 0;
    public string? HrCode { get; set; }
    public bool IsSupportFiles { get; set; }
    public Guid DepartmentId { get; set; }
}
