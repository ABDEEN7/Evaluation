using Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Planing;

namespace Evaluation.DAL.Entities.OrganizationTrees;

public class DepartmentOrganizationTree : BaseEntities
{
    public Guid DepartmentId { get; set; }
    public Guid OrganizationTreeId { get; set; }
    public Department? Department { get; set; }
    public OrganizationTree? OrganizationTree { get; set; }
}
