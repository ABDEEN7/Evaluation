using  Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Calendars;

namespace Evaluation.DAL.Entities.Org;

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
