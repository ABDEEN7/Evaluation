namespace Evaluation.DAL.Entities.Authentication;

public class UserPartyTypeSignature
{
    public Guid UserPartyTypeId { get; set; }
    public UserPartyType? UserPartyType { get; set; }
    public byte[] Signature { get; set; } = null!;
}
