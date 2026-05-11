namespace Evaluation.SharedHelper.Dtos.TeamMemberDto.ReassignDto;

public class RequestReassignDto
{
    public Guid UserId { get; set; }
    public Guid ToUserId { get; set; }
    public List<Guid> EvaluationRequests { get; set; }
}
