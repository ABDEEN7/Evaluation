using Evaluation.DAL.Entities.AdminPanel;

namespace Evaluation.DAL.Entities.FormsModules;

public class Form : BaseEntities
{
    public string NameAr { get; set; } = null!;
    public  string NameEn { get; set; } = null!;
    public bool IsRopric { get; set; }
    public bool HasOneValue { get; set; }
    public Guid DepartmentId { get; set; }
    public Guid EvaluationPartyId { get; set; }
    public EvaluationParty? EvaluationParties { get; set; }
    public bool HasEvaluation { get; set; } // i chnage the name of WithEvaluation to HasEvaluation to be more clear and applying to best practice 
    public Guid CalcMethodId { get; set; }
    public CalcMethod? CalcMethod { get; set; }
    public bool HasNotes { get; set; }
    public bool HasItemNotes { get; set; }
}
