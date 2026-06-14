using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.Calendars;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Models.Planing;
using Evaluation.DAL.Models.Website;

namespace Evaluation.DAL.Models.DepartementEntites;

public class Department : EntityBase, IAuditLogEntity
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public string RoutingPath { get; set; } = null!;
    public string BackendName { get; set; } = null!;
    public string DepIcon { get; set; } = null!;
    public Guid? WebsiteAttachmentId { get; set; }
    public WebsiteAttachment? WebsiteAttachment { get; set; }
    public bool IsNDA { get; set; }
    public bool IsEvaluated { get; set; }
    public string? DescAr { get; set; }
    public string? DescEn { get; set; }
    public int OrderNo { get; set; } = 0;
    public string? DepImageFileNameAr { get; set; } = null!;
    public string? DepImageUiFileNameAr { get; set; } = null!;
    public string? DepImageBlobUrlAr { get; set; }
    public string? DepImageFileNameEn { get; set; } = null!;
    public string? DepImageUiFileNameEn { get; set; } = null!;
    public string? DepImageBlobUrlEn { get; set; }
    public string? DepConfig { get; set; }
    public virtual ICollection<UserDepartment>? UserDepartments { get; set; }
    public virtual ICollection<AcademicYear>? AcademicYears { get; set; }
    public virtual ICollection<WebGroup>? WebGroup { get; set; }
    public virtual ICollection<DepTargetOrgTree>? DepTargetOrgTrees { get; set; }
    public virtual ICollection<SystemModule>? SystemModules { get; set; }

}