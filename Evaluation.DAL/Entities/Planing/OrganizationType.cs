using Evaluation.DAL.Entities.AdminPanel;
using Evaluation.DAL.Entities.OrganizationTrees;

namespace Evaluation.DAL.Entities.Planing;

public class OrganizationType : BaseEntities
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public ICollection<Organization> Organizations { get; set; } = new List<Organization>();
}
