using Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Planing;

namespace Evaluation.DAL.Entities.Org;

public class DepartmentOrganizationTree : EntityBase
{
    public Guid DepartmentId { get; set; }
    public Guid OrganizationTreeId { get; set; }
    public Department? Department { get; set; }
    public OrganizationTree? OrganizationTree { get; set; }
}
