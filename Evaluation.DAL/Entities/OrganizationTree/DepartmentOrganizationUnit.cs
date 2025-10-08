using Evaluation.DAL.Entities.AdminPanel;
using Evaluation.DAL.Entities.Planing;

namespace Evaluation.DAL.Entities.OrganizationTree;

public class DepartmentOrganizationUnit : BaseEntities //i change the name of DepartmentTarget to DepartmentOrganizationUnit
{
    public Guid DepartmentId { get; set; }
    public Guid OrganizationUnitId { get; set; }
    public Department? Department { get; set; }
    public OrganizationUnit? OrganizationUnit { get; set; }
}
