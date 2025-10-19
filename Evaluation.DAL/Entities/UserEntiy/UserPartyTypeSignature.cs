using Evaluation.DAL.Entities.Audit;
using  Evaluation.DAL.Entities.BaseModule;

namespace Evaluation.DAL.Entities.Authentication;

public class UserPartyTypeSignature : EntityBase, IAuditLogEntity
{
    public Guid UserPartyTypeId { get; set; }
    public UserPartyType? UserPartyType { get; set; }
    public byte[] Signature { get; set; } = null!;
}
