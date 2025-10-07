namespace Evaluation.DAL.Entities.OrganizationTree;

public class Employee : OrganizationUnit
{
    public string EmployeeNo { get; set; }
    public string Gender { get; set; }
    public DateTime BirthDate { get; set; }
    public string Nationality { get; set; }
    public DateTime JoinDate { get; set; }
    public DateTime StartDate { get; set; }
    public string JobTitle { get; set; }
    //Warning
    public int OrganizationId { get; set; }
    public int SubOrganizationId { get; set; }
}
