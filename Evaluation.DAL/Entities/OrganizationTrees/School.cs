using Evaluation.DAL.Entities.BaseModule;

namespace Evaluation.DAL.Entities.OrganizationTrees;

public class School : BaseEntities
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public DateTime EstablishmentDate { get; set; }
    public string? Address { get; set; }
    public string Code { get; set; } = null!;
    public string? Region { get; set; }
    public int TypeId { get; set; }//بنين وبنات
    public SchoolType SchoolType { get; set; } = new();
    //public List<SchoolLevel> SchoolLevel { get; set; } disscucss with FT to add new relation ship between level and school 
}
