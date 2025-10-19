namespace Evaluation.DAL.Entities.Org;

public class Employee : OrgTree
{
    public string EmployeeNo { get; set; } = null!;
    public string Gender { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public string Nationality { get; set; }=null!;
    public DateTime JoinDate { get; set; }
    public string JobTitle { get; set; } = null!;
    public int OrganizationId { get; set; }
    public int SubOrganizationId { get; set; }
}
