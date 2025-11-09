using  Evaluation.DAL.Entities.BaseModule;

namespace Evaluation.DAL.Entities.Org;

public class School : OrgTree
{
    public Guid TypeId { get; set; }//بنين وبنات
    public SchoolType SchoolType { get; set; } = new();
    public DateOnly EstablishmentDate { get; set; }
    public string? ManagerQID { get; set; }
    public string? ManageEmail { get; set; }
    public string? OrgEmail { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Mobile { get; set; }
    public string Code { get; set; } = null!;
    public string? Region { get; set; }

    //public List<SchoolLevel> SchoolLevel { get; set; } disscucss with FT to add new relation ship between level and school 
}
