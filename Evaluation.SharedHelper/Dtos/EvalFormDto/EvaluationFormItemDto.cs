
using Evaluation.DAL.Models.FormsModules;
using Evaluation.SharedHelper.Models;

namespace Evaluation.SharedHelper.Dtos.EvalFormDto;

public class EvaluationFormItemDto : EntityBaseDTO
{
    public Guid EvalFormId { get; set; }
    public string? EvalForm { get; set; }
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public decimal Min { get; set; } = 0;
    public decimal Max { get; set; } = 0;
    public bool IsEvaluation { get; set; }
    public decimal Weight { get; set; } = 0;
    public Guid ScopeId { get; set; }
    public string? Scope { get; set; }
    public Guid? DropDownTypeId { get; set; } 
    public string? DropDownType { get; set; } 
    public Guid CalcMethodId { get; set; }
    public string? CalcMethod { get; set; }
    public bool HasNote { get; set; } = false;
    public bool NoteRequired { get; set; }
    public virtual ICollection<EvaluationFormSubItemDto>? SubFormItems { get; set; }
    public Guid[]? FormItemRelated { get; set; }
    public Guid? AnalysisTypeId { get; set; }
    public bool HasMulitEvaluation { get; set; }
}