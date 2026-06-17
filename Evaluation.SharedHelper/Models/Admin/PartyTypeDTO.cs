


namespace Evaluation.SharedHelper.Models.Admin;
public class PartyTypeDTO : EntityBaseDTO
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public string BackendName { get; set; } = null!;
    public bool IsEmployeePartyType { get; set; }
    public bool CanViewAllRequests { get; set; }
    public bool CanViewAllEvaluations { get; set; }
    public Guid DepartmentId { get; set; }
    public string? Department { get; set; }
    public Guid[]? UserPartyType { get; set; }
    public bool CanViewAllPlan { get; set; }
}
