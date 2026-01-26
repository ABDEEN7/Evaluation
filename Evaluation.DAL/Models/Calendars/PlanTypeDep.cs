using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.DepartementEntites;

namespace Evaluation.DAL.Models.Calendars;

public class PlanTypeDep : EntityBase, IAuditLogEntity
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public Guid DepartmentId { get; set; }
    public Department? Department { get; set; }
    public Guid PlanTypeId { get; set; }
    public PlanType? PlanType { get; set; }
    public int OrderNo { get; set; } = 0;

}