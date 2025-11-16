using System.Reflection.PortableExecutable;
using  Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Authentication;
using  Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Audit;

namespace Evaluation.DAL.Entities.FormsModules;
public class FormItemValue : EntityBase ,IAuditLogEntity
{
    public Guid FormItemId { get; set; }
    public FormItem? FormItem { get; set; }
    public Guid UserId { get; set; }
    public MinistryUser? User { get; set; }
    public decimal Value { get; set; }
    public string? Note { get; set; }
}
