using System.Reflection.PortableExecutable;
using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.EvalResult;
using Evaluation.DAL.Models.UserEntiy;

namespace Evaluation.DAL.Models.FormsModules;
public class FormItemValue : EntityBase ,IAuditLogEntity
{
    public Guid FormItemId { get; set; }
    public FormItem? FormItem { get; set; }
    public Guid UserId { get; set; }
    public MinistryUser? User { get; set; }
    public Guid? DepEvalMatrixId { get; set; }
    public DepEvalMatrix? DepEvalMatrix { get; set; }
    public string? DepEvalMatrixValue { get; set; }
    public decimal? Value { get; set; }
    public string? Note { get; set; }
}
