using Evaluation.DAL.Entities.BaseModule;

namespace Evaluation.DAL.Entities.Planing;

public class ChangeRequestType : EntityBase
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public ICollection<ChangeRequest>? ChangeRequests { get; set; }
}