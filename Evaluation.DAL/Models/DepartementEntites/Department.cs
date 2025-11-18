using  Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Calendars;
using Evaluation.DAL.Entities.Org;
using Evaluation.DAL.Entities.Planing;

namespace Evaluation.DAL.Models.DepartementEntites;

public class Department : EntityBase
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public string RoutingPath { get; set; } = null!;
    public string BackendName { get; set; } = null!;
    public string DepIcon { get; set; } = null!;
    public Guid? TargetOrgTreeId { get; set; }
    public OrgTree? TargetOrgTree { get; set; }
    public Category? Category { get; set; }
    public Guid CategoryId { get; set; }
    public bool IsNDA { get; set; }
    public string? DescAr { get; set; }
    public string? DescEn { get; set; }
    public int OrderNo { get; set; } = 0;
    public ICollection<AcademicYear> AcademicYears { get; set; }
}