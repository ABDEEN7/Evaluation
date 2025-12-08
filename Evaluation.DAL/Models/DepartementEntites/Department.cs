using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.Calendars;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Models.Planing;
using Evaluation.DAL.Models.Website;

namespace Evaluation.DAL.Models.DepartementEntites;

public class Department : EntityBase
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public string RoutingPath { get; set; } = null!;
    public string BackendName { get; set; } = null!;
    public string DepIcon { get; set; } = null!;
    public Guid? WebsiteAttachmentId { get; set; }
    public WebsiteAttachment? WebsiteAttachment { get; set; }
    public Guid? TargetOrgTreeId { get; set; }
    public OrgTree? TargetOrgTree { get; set; }
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
    public bool IsNDA { get; set; }
    public string? DescAr { get; set; }
    public string? DescEn { get; set; }
    public int OrderNo { get; set; } = 0;
    public virtual ICollection<UserDepartment>? UserDepartments { get; set; }
    public virtual ICollection<AcademicYear>? AcademicYears { get; set; }
}