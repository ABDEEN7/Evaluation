using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;

namespace Evaluation.DAL.Models.UserEntiy;

public class UserPartyTypeSignature : EntityBase, IAuditLogEntity
{
    public Guid UserPartyTypeId { get; set; }
    public UserPartyType? UserPartyType { get; set; }
    public byte[] Signature { get; set; } = null!;
}
