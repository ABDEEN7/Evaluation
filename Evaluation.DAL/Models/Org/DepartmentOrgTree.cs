using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.DepartementEntites;

namespace Evaluation.DAL.Models.Org;

public class DepartmentOrgTree : EntityBase
{
    public Guid DepartmentId { get; set; }
    public Department? Department { get; set; }
    public Guid OrganizationTreeId { get; set; }
    public OrgTree? OrgTree { get; set; }
}
