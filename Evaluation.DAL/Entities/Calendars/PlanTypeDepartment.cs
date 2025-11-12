using  Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.DepartementEntites;
using Evaluation.DAL.Entities.Planing;

namespace Evaluation.DAL.Entities.Calendars;

public class PlanTypeDepartment : EntityBase
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public Guid DepartmentId { get; set; }
    public Department? Department { get; set; }
    public Guid PlanTypeId { get; set; }
    public PlanType? PlanType { get; set; }
    public int OrderNo { get; set; } = 0;

}