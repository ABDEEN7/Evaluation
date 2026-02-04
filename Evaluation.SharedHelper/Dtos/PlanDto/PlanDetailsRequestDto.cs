using Evaluation.SharedHelper.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.SharedHelper.Dtos.PlanDto;

public class PlanDetailsRequestDto : PaginatedQuery
{
    public Guid? PlanId { get; set; }
    public Guid? YearId { get; set; }
    public string? SchoolName { get; set; }
}
