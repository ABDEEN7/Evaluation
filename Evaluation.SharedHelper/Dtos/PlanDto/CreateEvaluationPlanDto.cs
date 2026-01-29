using Evaluation.DAL.Models.Planing;
using Evaluation.SharedHelper.Dtos.SchoolDto;

namespace Evaluation.SharedHelper.Dtos.PlanDto;

public class CreateEvaluationPlanDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid PlanTypeDepId { get; set; }
    public List<SelectedSchool>? Schools { get; set; }
    public Guid? AcademicYearId { get; set; }
    public Guid? PlanStatusId { get; set; }
    public Guid? SemesterId { get; set; }

    public Plan ToPlan()
    {
        return new Plan
        {
            Id = Id,
            PlanName = Name,
            StartDate = DateOnly.FromDateTime(StartDate),
            EndDate = DateOnly.FromDateTime(EndDate),
            //PlanTypeDepartmentId = PlanTypeId,
            PlanTypeDepId = PlanTypeDepId,
            AcademicYearId = AcademicYearId,
            PlanStatusId = PlanStatusId.Value,
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
            StartDate = DateOnly.FromDateTime(request.StartDate),
            EndDate = DateOnly.FromDateTime(request.EndDate),
            PlanTypeDepId = request.PlanTypeDepId,
            Schools = request.Schools,
            AcademicYearId = request.AcademicYearId,
            PlanStatusId = request.PlanStatusId.Value,
            SemesterId = request.SemesterId
        };
    }
}