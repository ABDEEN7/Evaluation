using Evaluation.DAL.Entities.Planing;

namespace Evaluation.DAL.Entities.FormsModules;

public class ScopeType
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public int DepartmentId { get; set; }
    public Department Department { get; set; } = new();
    public int Order { get; set; }
}
