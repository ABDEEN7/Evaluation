using Evaluation.DAL.Entities.Authentication;
using Evaluation.DAL.Entities.BaseModule;


namespace Evaluation.DAL.Entities.Planing;

public class ChangeRequest : EntityBase
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public Guid RequestTypeId { get; set; }
    public ChangeRequestType? ChangeRequestType { get; set; }
    public Guid PlanId { get; set; }
    public Plan? Plan { get; set; }
    public Guid RequestedById { get; set; }
    public MinistryUser? User { get; set; }
    public string? Notes { get; set; }
}
