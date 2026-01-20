namespace Evaluation.SharedHelper.Dtos.HrDto;

public class DepartmentHolidayOrgDto
{
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsCronExpression { get; set; }
    public string? CronExpression { get; set; } = string.Empty;
}
