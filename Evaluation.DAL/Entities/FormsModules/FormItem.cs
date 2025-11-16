using Evaluation.DAL.Entities.Audit;
using  Evaluation.DAL.Entities.BaseModule;

namespace Evaluation.DAL.Entities.FormsModules;

public class FormItem : EntityBase, IAuditLogEntity
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public decimal Min { get; set; }
    public decimal Max { get; set; }
    public bool IsEvaluation{ get; set; }
    public int Weight { get; set; }
    public Guid EvalFormId { get; set; }
    public EvalForm? EvalForm { get; set; }
    public Guid ScopeId { get; set; }
    public Scope? Scope { get; set; }
    //public Guid Type { get; set; }
    //public Guid DropDownType { get; set; } // if activate Min Max or not and if mkae questioning or not
    public Guid CalcMethodId { get; set; }
    public CalcMethod? CalcMethod { get; set; }
}