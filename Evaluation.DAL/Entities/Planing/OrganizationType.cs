using Evaluation.DAL.Entities.AdminPanel;
using Evaluation.DAL.Entities.OrganizationTree;

namespace Evaluation.DAL.Entities.Planing;

public class OrganizationType : BaseEntities
{
    public required string NameAr { get; set; }
    public required string NameEn { get; set; }
    public ICollection<Organization> Organizations { get; set; } = new List<Organization>();
}
