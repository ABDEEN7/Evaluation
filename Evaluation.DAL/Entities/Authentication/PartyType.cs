namespace Evaluation.DAL.Entities.Authentication;

public class PartyType
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public string? BackendName { get; set; }
    public bool IsEmployeePartyType { get; set; }
    public bool CanViewAllRequests { get; set; }
    public bool CanViewAllEvaluations { get; set; }
    public bool CanViewEntityEvaluation { get; set; }
    public Guid SystemModuleId { get; set; }
    /// i don't take anything about this 
    //public Departement? Departement { get; set; }
    //public Guid? DepartementId? { get; set; }
}
