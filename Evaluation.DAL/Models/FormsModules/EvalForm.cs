using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;

namespace Evaluation.DAL.Models.FormsModules;

public class EvalForm : EntityBase , IAuditLogEntity
{
    public Guid EvalFormTypeId { get; set; }
    public EvalFormType? EvalFormType { get; set; }
    public Guid FormEvalMatrixId { get; set; }
    public FormEvalMatrix? FormEvalMatrix { get; set; }
    public string NameAr { get; set; } = null!;
    public  string NameEn { get; set; } = null!;
    public bool HasOneValue { get; set; }
    public Guid EvaluationPartyId { get; set; }
    public EvaluationParty? EvaluationParties { get; set; }
    public bool HasEvaluation { get; set; }
    public Guid CalcMethodId { get; set; }
    public CalcMethod? CalcMethod { get; set; }
    public Guid FormStatusId { get; set; }
    public FormStatus? FormStatus { get; set; } 
    public bool IsFinalEval { get; set; }

}
