

using Evaluation.SharedHelper.Models;

namespace Evaluation.SharedHelper.Dtos.EvalFormDto;

public class TemplateFormDto : EntityBaseDTO
{
    public Guid EvalFormTypeId { get; set; }
    public string? EvalFormType { get; set; }
    public Guid FormEvalMatrixId { get; set; }
    public string? FormEvalMatrix { get; set; }
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public bool IsFinalEval { get; set; }
    public bool HasOneValue { get; set; }
    public Guid EvaluationPartyId { get; set; }
    public string? EvaluationParty { get; set; }
    public bool HasEvaluation { get; set; }
    public Guid CalcMethodId { get; set; }
    public string? CalcMethod { get; set; }
    public string? FormStatus { get; set; }
    public Guid FormStatusId { get; set; }
    public Guid? FinalEvalMatrixId { get; set; }
    public bool AllowRename { get; set; }
    public bool HasMuliEvaluation { get; set; }
    public int? EvalCountOfColumnsValue { get; set; } = 1;
}