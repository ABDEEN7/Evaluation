namespace Evaluation.DAL.Entities.OrganizationTrees;

public class Employee : OrganizationTree
{
    public string EmployeeNo { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Nationality { get; set; } = string.Empty;
    public DateTime JoinDate { get; set; }
    public string JobTitle { get; set; } = null!;
    //Warning
    public int OrganizationId { get; set; }
    public int SubOrganizationId { get; set; }
}
