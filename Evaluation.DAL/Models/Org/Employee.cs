
using Evaluation.DAL.Models.Master;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Evaluation.DAL.Models.Org;

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
    public string QID { get; set; } = null!;


}
