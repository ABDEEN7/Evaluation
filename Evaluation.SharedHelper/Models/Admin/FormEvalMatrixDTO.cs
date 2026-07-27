namespace Evaluation.SharedHelper.Models.Admin;

public class FormEvalMatrixDTO : EntityBaseDTO
{
    public Guid DepartmentId { get; set; }
    public string? Department { get; set; }
    public string BackenName { get; set; } = null!;
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public DateOnly Startdate { get; set; }
    public DateOnly? EndDate { get; set; }
    public bool RequiredFollowUp { get; set; }
}