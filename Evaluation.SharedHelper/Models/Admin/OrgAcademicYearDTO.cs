using Evaluation.DAL.Models.Org;

namespace Evaluation.SharedHelper.Models.Admin;

public class OrgAcademicYearDTO : EntityBaseDTO
{
    public Guid OrgTreeId { get; set; }
    public string OrgTree { get; set; }
    public Guid ParentOrgTreeId { get; set; }
    public string? ParentOrgTree { get; set; }
    public string? JobTitleAr { get; set; }
    public string? JobTitleEn { get; set; }
    public int Year { get; set; }
}