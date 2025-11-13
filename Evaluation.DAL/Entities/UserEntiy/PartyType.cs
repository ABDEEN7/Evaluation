using Evaluation.DAL.Entities.Audit;
using  Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.DepartementEntites;
using Evaluation.DAL.Entities.SystemModulesEntities;
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
    public Guid SystemModuleId { get; set; }
    public SystemModule? SystemModule { get; set; }
	//public Guid? DepartmentId { get; set; }
 //   public Department? Department { get; set; }
}
