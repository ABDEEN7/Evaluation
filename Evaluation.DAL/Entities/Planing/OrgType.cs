using  Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Org;

namespace Evaluation.DAL.Entities.Planing;

public class OrgType : EntityBase
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public virtual ICollection<Organization>? Organizations { get; set; }
}
