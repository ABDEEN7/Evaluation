using Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Planing;

namespace Evaluation.DAL.Entities.Org;

public class OrgTree : EntityBase
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public Guid ParentTreeId { get; set; }
    public OrgTree? ParentTree { get; set; }
    public string? HrCode { get; set; } 
    public string? NSISCode { get; set; } 
}
