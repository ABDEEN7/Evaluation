using Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Masters;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Evaluation.DAL.Entities.Org;

public class Employee : OrgTree
{
    public string EmployeeNo { get; set; } = null!;
	public Guid UserGenderId { get; set; }
	public UserGender? UserGender { get; set; }
	public DateOnly BirthDate { get; set; }
    public string NationalityCode { get; set; }=null!;
    public DateOnly JoinDate { get; set; }
    public Guid JobTitleId { get; set; } 
    public JobTitle JobTitle { get; set; } = null!;
    public string? Email { get; set; }


}
