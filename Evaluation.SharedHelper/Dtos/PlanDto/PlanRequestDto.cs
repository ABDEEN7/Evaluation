using Evaluation.SharedHelper.Enums;

namespace Evaluation.SharedHelper.Dtos.PlanDto;

public class PlanRequestDto : PaginatedQuery
{
    public string? PlanName { get; set; }
    public Guid? StatusId { get; set; }
}
