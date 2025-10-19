using System.Reflection.PortableExecutable;
using  Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Authentication;
using  Evaluation.DAL.Entities.BaseModule;

namespace Evaluation.DAL.Entities.FormsModules;
//هذا التايبل يخزن قيمة المستخدم والبند المقييم
public class ItemValue : EntityBase
{
    public Guid ItemId { get; set; }
    public Guid UserId { get; set; }
    public Item? Item { get; set; }
    public MinistryUser? User { get; set; }
    public decimal Value { get; set; }
}
