using Evaluation.DAL.Models.BaseModule;

namespace Evaluation.DAL.Models.Org;

public class OrgAcademicYear : EntityBase
{
    public Guid OrgTreeId { get; set; }
    public OrgTree? OrgTree { get; set; }
    public Guid? ParentOrgTreeId { get; set; }
    public OrgTree? ParentOrgTree { get; set; }
    public string? JobTitleAr { get; set; } // to keep the jobtitle history for the empoloyees
    public string? JobTitleEn { get; set; }
    public int Year { get; set; }
}
