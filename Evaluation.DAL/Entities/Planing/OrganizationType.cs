using Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.OrganizationTrees;

namespace Evaluation.DAL.Entities.Planing;

public class OrganizationType : EntityBase
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public ICollection<Organization>? Organizations { get; set; }
}
