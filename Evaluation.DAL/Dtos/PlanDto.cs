using Evaluation.DAL.Models.Planing;

namespace Evaluation.DAL.Dtos;

public class PlanDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public Guid PlanTypeId { get; set; }
    //public List<SelectedSchool>? Schools { get; set; }
    public Guid AcademicYearId { get; set; }
    public Guid PlanStatusId { get; set; }
    public Guid? SemesterId { get; set; }
    public static PlanDto FromEntity(Plan plan)
    {
        if (plan == null)
            throw new ArgumentNullException(nameof(plan));

        return new PlanDto
        {
            Id = plan.Id,
            Name = plan.PlanName,
            StartDate = plan.StartDate,
            EndDate = plan.EndDate,
            //PlanTypeId = plan.PlanTypeId,
            AcademicYearId = plan.AcademicYearId,
            PlanStatusId = plan.PlanStatusId,
            SemesterId = plan.SemesterId
        };
    }
}
