using Evaluation.DAL.Entities.BaseModule;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Evaluation.DAL.Entities.Org;

public class Employee : OrgTree
{
    public string EmployeeNo { get; set; } = null!;
    public string Gender { get; set; } = null!;
    public DateOnly BirthDate { get; set; }
    public string NationalityCode { get; set; }=null!;
    public DateOnly JoinDate { get; set; }
    public string JobTitle { get; set; } = null!;
    //public Guid OrganizationId { get; set; }
    //public Organization? Organization { get; set; }
}
