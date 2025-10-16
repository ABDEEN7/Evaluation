using Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Org;
using Evaluation.DAL.Entities.Planing;

namespace Evaluation.DAL.Entities.Template;

public class ServiceRequest : EntityBase
{
    public int RequestNo { get; set; }
    public Guid PlanId { get; set; }
    public Plan? Plan { get; set; }
    public Guid StatusId { get; set; }
    public StatusService? Status{ get; set; }
    public Service? Service { get; set; }
    public Service? ServiceId { get; set; }
    public Guid OrganizationId { get; set; }
    public Organization? Organization { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
