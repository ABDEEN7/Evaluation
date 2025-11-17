using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;

namespace Evaluation.DAL.Models.FormsModules;

public class FormScope : EntityBase, IAuditLogEntity
{
    public Guid EvalFormId { get; set; }
    public EvalForm? EvalForm { get; set; }

    public Guid ScopeId { get; set; }
    public Scope? Scope { get; set; }
    
    public decimal Wegiht { get; set; }
}
