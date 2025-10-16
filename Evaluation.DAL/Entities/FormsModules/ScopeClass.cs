using Evaluation.DAL.Entities.Planing;

namespace Evaluation.DAL.Entities.FormsModules;

public class ScopeClass
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public int ParentId { get; set; }
    public ScopeClass? Parent { get; set; }
    public int DepartmentId { get; set; }
    public Department? Department { get; set; }
    public int OrderNo { get; set; }

}
