using Evaluation.SharedHelper.Models.Api.ServiceDTOs;

namespace Evaluation.SharedHelper.Dtos.PlanDto;

public class PlanListDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string? StatusCode { get; set; }
    public int? CountSchools { get; set; }
    public Guid PlanStatusId { get; set; }

	public IList<ServiceDTO>? Services { get; set; }
}
