using Evaluation.DAL.Entities.BaseModule;

namespace Evaluation.DAL.Entities.Template;

public class Service : EntityBase
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public SystemModule? SystemModule { get; set; }
    public Guid SystemModuleId { get; set; }
    public Guid DepartmentEvaluationPartyId { get; set; }
    //public DepartmentEvaluationParty DepartmentEvaluationParty { get; set; }
}
