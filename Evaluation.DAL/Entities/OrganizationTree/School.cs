using Evaluation.DAL.Entities.Base;

namespace Evaluation.DAL.Entities.OrganizationTree;

public class School : BaseEntities
{
    public DateTime EstablishmentDate { get; set; }
    public string Address { get; set; }
    public string Code { get; set; }
    public string Region { get; set; }
    public int TypeId { get; set; }
    public SchoolType SchoolType { get; set; }
    //public List<SchoolLevel> SchoolLevel { get; set; } disscucss with FT to add new relation ship between level and school 
}
