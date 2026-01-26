using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.Org;

namespace Evaluation.DAL.Models.Planing;

public class OrgType : EntityBase, IAuditLogEntity
{
    public string BackendName { get; set; } = null!;
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public virtual ICollection<Organization>? Organizations { get; set; }
}
