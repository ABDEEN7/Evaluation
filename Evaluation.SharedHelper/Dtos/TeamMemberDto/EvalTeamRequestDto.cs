using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.SharedHelper.Dtos.TeamMemberDto;

public class EvalTeamRequestDto
{
    public Guid UserId { get; set; }
    public Guid EvaluationRequestId { get; set; }
    public Guid PartyTypeId { get; set; }
    public bool IsLeader { get; set; }
    public string? Note { get; set; }
    public List<EvalScopesDto>? Scopes { get; set; }

}
