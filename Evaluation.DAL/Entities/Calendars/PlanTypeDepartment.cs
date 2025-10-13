using Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Planing;

namespace Evaluation.DAL.Entities.Calendars;

public class PlanTypeDepartment : BaseEntities
{
    public Guid PlanTypeId { get; set; }
    public PlanType? PlanType { get; set; }
    public Guid OrderNo { get; set; }
    public Guid DepartmentId { get; set; }
    public Department? Department { get; set; }
}