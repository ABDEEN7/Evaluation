using Evaluation.DAL.Entities.Planing;

namespace Evaluation.DAL.Entities.FormsModules;

public class ScopeType
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public int ParentId { get; set; }
    public ScopeType? Parent { get; set; }
    public int DepartmentId { get; set; }
    public Department? Department { get; set; }
    public int OrderNo { get; set; }

}
