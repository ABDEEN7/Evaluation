using Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Planing;

namespace Evaluation.DAL.Entities.FormsModules;

public class Scope : BaseEntities
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public int Order { get; set; }
    public Guid DepartmentId { get; set; }
    public Guid ScopeTypeId { get; set; }
    public Department? Department { get; set; }
    public ScopeType? ScopeType { get; set; }
}