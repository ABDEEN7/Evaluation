using Evaluation.DAL.Entities.BaseModule;

namespace Evaluation.DAL.Entities.Planing;

public class Department : EntityBase
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public Guid OrganizationTypeId { get; set; } 
    public OrganizationType? OrganizationType { get; set; } 
    //public List<Section> Sections { get; set; }
}