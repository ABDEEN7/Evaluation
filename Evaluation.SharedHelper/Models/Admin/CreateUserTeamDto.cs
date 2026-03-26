namespace Evaluation.SharedHelper.Models.Admin;

public class CreateUserTeamDto
{
    public Guid UserId { get; set; }
    public List<Guid> ScopeIds { get; set; } = new();
}
