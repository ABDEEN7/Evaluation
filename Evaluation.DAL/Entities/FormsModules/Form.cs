using Evaluation.DAL.Entities.AdminPanel;

namespace Evaluation.DAL.Entities.FormsModules;

public class Form : BaseEntities
{
    public required string NameAr { get; set; } 
    public required string NameEn { get; set; }
    public Guid DepartmentId { get; set; }
    public Guid EvaluationPartyId { get; set; }
    public bool HasEvaluation { get; set; } // i chnage the name of WithEvaluation to HasEvaluation to be more clear and applying to best practice 
    public Guid CalcMethodId { get; set; } // this is not perfect for me
    public bool IsRopric { get; set; }
}
