namespace Evaluation.SharedHelper.Dtos.TeamMemberDto;

public class EvaluationRequestAssignmentDto
{
    public Guid MinistryUserId { get; set; }
    public string MinistryUser { get; set; }
    public Guid EvaluationRequestId { get; set; }
    public Guid PartyTypeId { get; set; }
    public string PartyType { get; set; }
    public bool IsLeader { get; set; }
    public bool IsNDA { get; set; }
    public Guid? NdaStatusId { get; set; }
    public string? NdaStatus { get; set; }
    public DateTime? NdaDate { get; set; }
    public string? Note { get; set; }
    public ICollection<EvalRequestAssignmentScopeDto>? EvalRequestAssignmentScopies { get; set; }
}
