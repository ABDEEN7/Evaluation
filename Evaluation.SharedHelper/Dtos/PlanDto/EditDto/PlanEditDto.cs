namespace Evaluation.SharedHelper.Dtos.PlanDto.EditDto;

public class PlanEditDto
{
    public Guid Id { get; set; }
    public string PlanName { get; set; } = null!;
    public Guid? PlanTypeDepId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }

    public List<SchoolEvaluationEditDto> Schools { get; set; } = new();
}

