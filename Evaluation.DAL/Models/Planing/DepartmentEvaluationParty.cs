using Evaluation.DAL.Models.BaseModule;

namespace Evaluation.DAL.Models.Planing;

public class DepartmentEvaluationParty : EntityBase
{
    public Guid MyProperty { get; set; }
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
}
