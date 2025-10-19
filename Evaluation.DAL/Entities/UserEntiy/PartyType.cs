using Evaluation.DAL.Entities.Audit;
using  Evaluation.DAL.Entities.BaseModule;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.DAL.Entities.Authentication;

[Index(nameof(BackendName), IsUnique = true)]

public class PartyType : EntityBase, IAuditLogEntity
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public string BackendName { get; set; } = null!;
    public bool IsEmployeePartyType { get; set; }
    public bool CanViewAllRequests { get; set; }
    public bool CanViewAllEvaluations { get; set; }
    public bool CanViewEntityEvaluation { get; set; }
    public Guid SystemModuleId { get; set; }
 
    //public Departement? Departement { get; set; }
    //public Guid? DepartementId? { get; set; }
}
