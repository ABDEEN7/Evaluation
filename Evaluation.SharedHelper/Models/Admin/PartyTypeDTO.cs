


namespace Evaluation.SharedHelper.Models.Admin;
public class PartyTypeDTO : EntityBaseDTO
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public string BackendName { get; set; } = null!;
    public bool IsEmployeePartyType { get; set; }
    public bool CanViewAllRequests { get; set; }
    public bool CanViewAllEvaluations { get; set; }
    public Guid SystemModuleId { get; set; }
    public string? SystemModule { get; set; }
    public Guid[]? UserPartyType { get; set; }

}
