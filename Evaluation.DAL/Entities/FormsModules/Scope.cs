using Evaluation.DAL.Entities.AdminPanel;
using Evaluation.DAL.Entities.Planing;

namespace Evaluation.DAL.Entities.FormsModules;

public class Scope : BaseEntities
{
    public string? NameAr { get; set; }
    public string? NameEn { get; set; }
    public int Order { get; set; }
    public int ParentId { get; set; }
    public Guid DepartmentId { get; set; }
    public int ScopeTypeId { get; set; }
    public Department? Department { get; set; }
    public ScopeType? ScopeType { get; set; }
}