using  Evaluation.DAL.Entities.BaseModule;

namespace Evaluation.DAL.Entities.Org;

public class OrgTree : EntityBase
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public Guid OrgTreeId { get; set; }
    public OrgTree? Parent { get; set; }
    public string? HrCode { get; set; } 
    public string? NSISCode { get; set; }
    public Guid OrgTreeTypeId { get; set; }
    public OrgTreeClass OrgTreeClass { get; set; } = null!;
}