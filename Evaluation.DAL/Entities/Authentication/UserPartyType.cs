namespace Evaluation.DAL.Entities.Authentication;

public class UserPartyType
{
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public Guid PartyTypeId { get; set; }
    public PartyType? PartyType { get; set; }
    public bool? ShowInquires { get; set; }
    public bool? ShowAllPartyType { get; set; }

    public string? SignaturePlaceHolder { get; set; }
}