using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;

namespace Evaluation.DAL.Models.UserEntiy;

public class UserPartyType : EntityBase, IAuditLogEntity
{
    public Guid UserId { get; set; }
    public MinistryUser? User { get; set; }
    public Guid PartyTypeId { get; set; }
    public PartyType? PartyType { get; set; }
    public string? SignaturePlaceHolder { get; set; }
}