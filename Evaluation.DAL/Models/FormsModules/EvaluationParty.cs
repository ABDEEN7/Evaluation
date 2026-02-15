using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.DepartementEntites;

namespace Evaluation.DAL.Models.FormsModules;

public class EvaluationParty : EntityBase, IAuditLogEntity
{
    public Guid DepartmentId { get; set; }
    public Department? Department { get; set; }
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public bool IsSupportFiles { get; set; }
    public int OrderNo { get; set; }
 
}
