
namespace Evaluation.SharedHelper.Models.Admin;
public class PartyTypeDTO : EntityBaseDTO
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public string? BackendName { get; set; }
    public bool IsEmployeePartyType { get; set; }
    public bool CanViewAllRequests { get; set; }
    public bool CanViewAllScholarships { get; set; }
    public bool CanViewEntityScholarships { get; set; }
    public Guid SystemModuleId { get; set; }
    public string SystemModule { get; set; } = null!;
    public string? ParentPartyTypeId { get; set; }
    public string? ParentPartyType { get; set; }
    public Guid[]? PartyTypeCulturalAttache { get; set; }
    public Guid[]? PartyTypeAllocation { get; set; }
    public Guid[]? PartyTypeBankAccount { get; set; }
    public Guid[]? UserPartyType { get; set; }

}
