namespace Evaluation.DAL.Models.Calendars;

public class ScheduleTemplate
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? SpecificDate { get; set; }
    public Guid PlanTypeId { get; set; }
    public PlanType ScheduleType { get; set; } = new();
}
