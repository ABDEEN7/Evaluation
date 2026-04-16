using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.FormsModules;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.DAL.Models.UserEntiy;

[Index(nameof(BackendName), IsUnique = true)]

public class PartyType : EntityBase, IAuditLogEntity
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public string BackendName { get; set; } = null!;
    public bool IsEmployeePartyType { get; set; }
    public bool CanViewAllRequests { get; set; }
    public bool CanViewAllEvaluations { get; set; }
    public Guid DepartmentId { get; set; }
    public Department? Department { get; set; }
    public bool IsAssignment { get; set; }
	public ICollection<PartyTypeEvalParty> PartyTypeEvalParties { get; set; } = new List<PartyTypeEvalParty>();

}
