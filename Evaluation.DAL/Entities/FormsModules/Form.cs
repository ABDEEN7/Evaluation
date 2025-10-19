using  Evaluation.DAL.Entities.BaseModule;

namespace Evaluation.DAL.Entities.FormsModules;

public class Form : EntityBase
{
    public string NameAr { get; set; } = null!;
    public  string NameEn { get; set; } = null!;
    public bool IsRopric { get; set; }
    public bool HasOneValue { get; set; }
    public Guid DepartmentId { get; set; }
    public Guid EvaluationPartyId { get; set; }
    public EvaluationParty? EvaluationParties { get; set; }
    public bool HasEvaluation { get; set; }
    public Guid CalcMethodId { get; set; }
    public CalcMethod? CalcMethod { get; set; }
    public FormStatus? FormStatus { get; set; } 
    public Guid FormStatusId { get; set; }

}
