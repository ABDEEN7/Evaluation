using Evaluation.SharedHelper.Models;

namespace Evaluation.SharedHelper.Dtos.EvalFormDto;

public class FormItemConfigDto : EntityBaseDTO
{
    public Guid EvalFormId { get; set; }
    public List<Guid>? FormItemIds { get; set; }
    public Guid PartyTypeId { get; set; }
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public Guid CalcMethodId { get; set; }
    public decimal Percentage { get; set; } = 0;
}
