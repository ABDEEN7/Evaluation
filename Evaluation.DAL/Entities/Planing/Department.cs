using Evaluation.DAL.Entities.AdminPanel;

namespace Evaluation.DAL.Entities.Planing;

public class Department : BaseEntities
{
    public required string NameAr { get; set; }
    public required string NameEn { get; set; }
    public Guid OrganizationTypeId { get; set; } // i change the CategoryId to SectorId  
    public OrganizationType? OrganizationType { get; set; } // i change the CategoryId to SectorId  
    //public List<Section> Sections { get; set; }
}