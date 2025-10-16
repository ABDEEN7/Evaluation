using Evaluation.DAL.Entities.Base;

namespace Evaluation.DAL.Entities.OrganizationTree;

public class OrganizationUnit : BaseEntities // i change the name of OrganizationTree to Unit to more clearity 
{
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public Guid OrgType { get; set; }
    public Guid ParentUnitId { get; set; }
    public string HrCode { get; set; }
    public string NSISCode { get; set; }
}
