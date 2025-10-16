using Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Planing;

namespace Evaluation.DAL.Entities.Org;

public class DepartmentOrgTree : EntityBase
{
    public Guid DepartmentId { get; set; }
    public Department? Department { get; set; }
    public Guid OrganizationTreeId { get; set; }
    public OrgTree? OrgTree { get; set; }
}
