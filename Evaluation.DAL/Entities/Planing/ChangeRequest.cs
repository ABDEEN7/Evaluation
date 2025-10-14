using Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Authentication;

namespace Evaluation.DAL.Entities.Planing;

public class ChangeRequest : EntityBase
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public int RequestTypeId { get; set; }
    public ChangeRequestType? ChangeRequestType { get; set; }
    public int PlanId { get; set; }
    public Plan? Plan { get; set; }
    public int RequestedById { get; set; }
    public User? User { get; set; }
    public string? Notes { get; set; }
}
