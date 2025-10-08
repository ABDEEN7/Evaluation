using Evaluation.DAL.Entities.AdminPanel;

namespace Evaluation.DAL.Entities.Planing;

public class ChangeRequestType : BaseEntities
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public ICollection<ChangeRequest>? ChangeRequests { get; set; }
}