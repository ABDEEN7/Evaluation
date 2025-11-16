using Evaluation.DAL.Entities.Audit;
using  Evaluation.DAL.Entities.BaseModule;

namespace Evaluation.DAL.Entities.FormsModules;

public class FormScope : EntityBase, IAuditLogEntity
{
    public Guid EvalFormId { get; set; }
    public EvalForm? EvalForm { get; set; }

    public Guid ScopeId { get; set; }
    public Scope? Scope { get; set; }
    
    public decimal Wegiht { get; set; }
}
