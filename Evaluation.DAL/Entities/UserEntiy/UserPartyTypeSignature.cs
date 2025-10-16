using Evaluation.DAL.Entities.Audit;
using Evaluation.DAL.Entities.Base;

namespace Evaluation.DAL.Entities.Authentication;

public class UserPartyTypeSignature : BaseEntities, IAuditLogEntity
{
    public Guid UserPartyTypeId { get; set; }
    public UserPartyType? UserPartyType { get; set; }
    public byte[] Signature { get; set; } = null!;
}
