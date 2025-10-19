using  Evaluation.DAL.Entities.BaseModule;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.DAL.Entities.Org;
[Index(nameof(BackendName), IsUnique = true)]
public class OrgTreeClass : EntityBase
{
    public Guid ParentId { get; set; }
    public OrgTreeClass? Parent { get; set; }
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public string BackendName { get; set; } = null!;
}
