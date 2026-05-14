using Evaluation.SharedHelper.Models;

namespace Evaluation.SharedHelper.Dtos.EvalFormDto;

public class FormItemConfigDto : EntityBaseDTO
{
    public Guid FormItemConfig_EvalFormId { get; set; }
    public List<Guid>? FormItemConfig_FormItemIds { get; set; }
    public Guid? FormItemConfig_PartyTypeId { get; set; }
    public string FormItemConfig_NameAr { get; set; } = null!;
    public string FormItemConfig_NameEn { get; set; } = null!;
    public Guid FormItemConfig_CalcMethodId { get; set; }
    public decimal FormItemConfig_Percentage { get; set; } = 0;
}
