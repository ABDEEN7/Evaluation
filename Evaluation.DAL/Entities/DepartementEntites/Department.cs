using  Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Planing;

namespace Evaluation.DAL.Entities.DepartementEntites;

public class Department : EntityBase
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public OrganizationType? OrganizationType { get; set; }
    public Guid OrganizationTypeId { get; set; } 
    public Category? Category { get; set; }
    public Guid CategoryId { get; set; }
    //public List<Section> Sections { get; set; } // تقييم مؤسسة او فرد او تيب
}