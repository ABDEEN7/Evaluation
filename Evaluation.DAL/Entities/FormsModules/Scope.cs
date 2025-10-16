using Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Planing;

namespace Evaluation.DAL.Entities.FormsModules;

public class Scope : EntityBase
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public Guid DepartmentId { get; set; }
    public Department? Department { get; set; }
    public Guid ScopeClassId { get; set; }
    public ScopeClass? ScopeType { get; set; }
    public int OrderNo { get; set; }
}