using  Evaluation.DAL.Entities.BaseModule;

namespace Evaluation.DAL.Entities.Planing;

public class DepartmentEvaluationParty : EntityBase
{
    public Guid MyProperty { get; set; }
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
}
