using Evaluation.SharedHelper.Dtos.SchoolDto;

namespace Evaluation.SharedHelper.Dtos.PlanDto;

public class CreateEvaluationPlanDto
{
    public string Name { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid PlanTypeId { get; set; }
    public List<SelectedSchool>? Schools { get; set; }
    public Guid AcademicYearId { get; set; }
    public Guid PlanStatusId { get; set; }

    public CreatePlanResponse ConvertFromRequestToResponse(CreateEvaluationPlanDto request)
    {
        return new CreatePlanResponse
        {
            Id = request.Id,
            Title = request.Title,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            PlanTypeId = request.PlanTypeId,
            Schools = request.Schools,
            AcademicYearId = request.AcademicYearId,
            PlanStatusId = request.PlanStatusId,
        };
    }
}