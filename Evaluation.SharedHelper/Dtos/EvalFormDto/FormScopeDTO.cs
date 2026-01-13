using Evaluation.SharedHelper.Models;

namespace Evaluation.SharedHelper.Dtos.EvalFormDto;

public class FormScopeDTO : EntityBaseDTO
{
    public Guid EvalFormId { get; set; }
    public string? EvalForm { get; set; }

    public Guid ScopeId { get; set; }
    public string? Scope { get; set; }
    
    public decimal Wegiht { get; set; }
}
