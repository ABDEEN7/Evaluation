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
    public Guid? DepEvalMatrixId { get; set; }   // Copy Matrid Id
    public DepEvalMatrix? DepEvalMatrix { get; set; }
    public Guid? FormEvalMatrixValueId { get; set; }
    public FormEvalMatrixValue? FormEvalMatrixValue { get; set; }
    public string? DepEvalMatrixValueName { get; set; }   // Copy Text of Matrix Name
    public decimal? ActualValue { get; set; }
    public string? Note { get; set; }
    public Guid? CalcMethodId { get; set; }
    public CalcMethod? CalcMethod { get; set; }
    public string? RenameItem { get; set; }
    public Guid? FormItemConfigId { get; set; }
    public FormItemConfig? FormItemConfig { get; set; }
}
