namespace Evaluation.DAL.Entities.Calendars;

public class ScheduleTemplate
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? SpecificDate { get; set; }
    public int ScheduleTypeId { get; set; }
    public ScheduleType ScheduleType { get; set; } = new();
}
