namespace Evaluation.SharedHelper.Models.Admin;

public class TeamDto : EntityBaseDTO
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public int OrderNo { get; set; } = 0;
    public Guid DepartmentId { get; set; }
    public string? Department { get; set; }

}