namespace Evaluation.SharedHelper.Models.Admin;

public class JobTitleDto : EntityBaseDTO
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public int OrderNo { get; set; } = 0;
}
