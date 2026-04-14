using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.FormBuilder;

namespace Evaluation.DAL.Models.FormsModules;

public class FormItem : EntityBase, IAuditLogEntity
{
    public Guid EvalFormId { get; set; }
    public EvalForm? EvalForm { get; set; }
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public decimal Min { get; set; } = 0;
    public decimal Max { get; set; } = 0;
    public bool IsEvaluation{ get; set; }
    public decimal Weight { get; set; } = 0;
    
    public Guid ScopeId { get; set; }
    public Scope? Scope { get; set; }
    public Guid? DropDownTypeId { get; set; } // if activate Min Max or not and if mkae questioning or not
    public DropDownType? DropDownType { get; set; } // if activate Min Max or not and if mkae questioning or not
    public bool HasNote { get; set; } 
    public bool NoteRequired { get; set; }
    public int OrderNo { get; set; } = 0;
    public string? ColorCode { get; set; }

    public virtual ICollection<SubFormItem>? SubFormItems { get; set; }
    public ICollection<FormItemRelated>? RelatedFrom { get; set; }
    public ICollection<FormItemRelated>? RelatedTo { get; set; }
}