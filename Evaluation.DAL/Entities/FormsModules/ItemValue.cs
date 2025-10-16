using System.Reflection.PortableExecutable;
using Evaluation.DAL.Entities.Authentication;
using Evaluation.DAL.Entities.Base;

namespace Evaluation.DAL.Entities.FormsModules;
//هذا التايبل يخزن قيمة المستخدم والبند المقييم
public class ItemValue : EntityBase
{
    public Guid ItemId { get; set; }
    public Guid UserId { get; set; }
    public Item? Item { get; set; }
    public User? User { get; set; }
    public decimal Value { get; set; }
}
