using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.DepartementEntites;

namespace Evaluation.DAL.Models.FormsModules;

public class ScopeType: EntityBase, IAuditLogEntity
{
    public string? BackendName  { get; set; }
    public string NameAr { get; set; } = null!;// معيار او مجال او جانب
    public string NameEn { get; set; } = null!;
    public Guid? ParentId { get; set; }
    public ScopeType? Parent { get; set; }
    public Guid DepartmentId { get; set; }
    public Department? Department { get; set; }
    public string? ColorCode { get; set; }
    public int OrderNo { get; set; }
}