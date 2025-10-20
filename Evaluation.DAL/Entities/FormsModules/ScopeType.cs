using Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.DepartementEntites;

namespace Evaluation.DAL.Entities.FormsModules;

public class ScopeType: EntityBase
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public Guid ParentId { get; set; }
    public ScopeType? Parent { get; set; }
    public Guid DepartmentId { get; set; }
    public Department? Department { get; set; }
    public int OrderNo { get; set; }
}