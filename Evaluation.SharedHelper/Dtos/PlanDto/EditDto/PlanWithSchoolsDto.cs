namespace Evaluation.SharedHelper.Dtos.PlanDto.EditDto;

public class PlanWithSchoolsDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public DateOnly startDate { get; set; }
    public DateOnly endDate { get; set; }
    public Guid? PlanTypeId { get; set; }
    public string? Rating { get; set; }
    public List<SchoolEvaluationDto> Schools { get; set; } = new();

}
