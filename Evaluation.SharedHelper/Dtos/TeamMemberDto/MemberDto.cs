namespace Evaluation.SharedHelper.Dtos.TeamMemberDto;

public class MemberDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<UserPartyTypeDto>? UserPartyTypes { get; set; }
    public string? JobTitle { get; set; }
}
public class UserPartyTypeDto
{
    public Guid Id { get; set; }
    public PartyTypeDto? PartyType { get; set; }
}
public class PartyTypeDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
}