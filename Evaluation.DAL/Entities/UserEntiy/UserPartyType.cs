using Evaluation.DAL.Entities.Audit;
using Evaluation.DAL.Entities.Base;

namespace Evaluation.DAL.Entities.Authentication;

public class UserPartyType : BaseEntities, IAuditLogEntity
{
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public Guid PartyTypeId { get; set; }
    public PartyType? PartyType { get; set; }
    public string? SignaturePlaceHolder { get; set; }
}