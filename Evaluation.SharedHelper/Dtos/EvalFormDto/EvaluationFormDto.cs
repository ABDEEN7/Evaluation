

using Evaluation.SharedHelper.Models;

namespace Evaluation.SharedHelper.Dtos.EvalFormDto;

public class EvaluationFormDto : EntityBaseDTO
{
    public Guid EvalFormTypeId { get; set; }
    public string? EvalFormType { get; set; }
    public Guid FormEvalMatrixId { get; set; }
    public string? FormEvalMatrix { get; set; }
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public bool IsRopric { get; set; }
    public bool HasOneValue { get; set; }
    public Guid EvaluationPartyId { get; set; }
    public string? EvaluationParty { get; set; }
    public bool HasEvaluation { get; set; }
    public Guid CalcMethodId { get; set; }
    public string? CalcMethod { get; set; }
    public string? FormStatus { get; set; }
    public Guid FormStatusId { get; set; }
}