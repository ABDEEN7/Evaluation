using Evaluation.SharedHelper.Dtos.PlanDto;

namespace Evaluation.SharedHelper.Dtos.WebSiteDto;

public class WebGroupsDto : BaseDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Desc { get; set; }
    public string? RoutingPath { get; set; }
    public string? BackendName { get; set; }

}