using Evaluation.DAL.Entities.BaseModule;

namespace Evaluation.DAL.Entities.OrganizationTrees;

public class OrganizationTree : BaseEntities
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public Guid OrganizationType { get; set; }
    public Guid ParentTreeId { get; set; }
    public OrganizationTree? ParentTree { get; set; }
    public string HrCode { get; set; } = string.Empty;
    public string NSISCode { get; set; } = string.Empty;
}
