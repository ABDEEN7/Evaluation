using Evaluation.DAL.Models.Planing;
using Evaluation.SharedHelper.Dtos.SchoolDto;

namespace Evaluation.SharedHelper.Dtos.PlanDto;

public class CreateEvaluationPlanDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public Guid PlanTypeId { get; set; }
    public List<SelectedSchool>? Schools { get; set; }
    public Guid AcademicYearId { get; set; }
    public Guid PlanStatusId { get; set; }
    public Guid? SemesterId { get; set; }

    public Plan ToPlan()
    {
        return new Plan
        {
            Id = Id,
            PlanName = Name,
            StartDate = StartDate,
            EndDate = EndDate,
            //PlanTypeDepartmentId = PlanTypeId,
            AcademicYearId = AcademicYearId,
            PlanStatusId = PlanStatusId,
            SemesterId = SemesterId,
            IsDeleted = false,
            CreateDate = DateTime.Now
        };
    }

    public CreatePlanResponse ConvertFromRequestToResponse(CreateEvaluationPlanDto request)
    {
        return new CreatePlanResponse
        {
            Id = request.Id,
            Name = request.Name,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            PlanTypeId = request.PlanTypeId,
            Schools = request.Schools,
            AcademicYearId = request.AcademicYearId,
            PlanStatusId = request.PlanStatusId,
            SemesterId = request.SemesterId
        };
    }
}