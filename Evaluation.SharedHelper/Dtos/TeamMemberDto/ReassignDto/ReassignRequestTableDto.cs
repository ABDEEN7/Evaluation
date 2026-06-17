using Evaluation.SharedHelper.Models;

namespace Evaluation.SharedHelper.Dtos.TeamMemberDto.ReassignDto;

public class ReassignRequestTableDto : EntityTableDTO
{
    public Guid EvaluationRequestId { get; set; }
    public string ServiceNameEn { get; set; }
    public string ServiceNameAr { get; set; }
    public string RequestNumber { get; set; }
    public Guid PartyTypeId { get; set; }
}
