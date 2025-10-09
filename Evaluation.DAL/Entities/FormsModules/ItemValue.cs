using System.Reflection.PortableExecutable;
using Evaluation.DAL.Entities.AdminPanel;
using Evaluation.DAL.Entities.Authentication;

namespace Evaluation.DAL.Entities.FormsModules;
//هذا التايبل يخزن قيمة المستخدم والبند المقييم
public class ItemValue : BaseEntities
{
    public int ItemId { get; set; }
    public int UserId { get; set; }
    public Item? Item { get; set; }
    public User? User { get; set; }
    public decimal Value { get; set; }
}
