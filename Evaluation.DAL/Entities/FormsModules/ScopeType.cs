using Evaluation.DAL.Entities.Planing;

namespace Evaluation.DAL.Entities.FormsModules;

public class ScopeType
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public int DepartmentId { get; set; }
    public int ParentId { get; set; }
    public int Order { get; set; }
    public ScopeType Parent { get; set; } = new();
    public Department Department { get; set; } = new();
}
