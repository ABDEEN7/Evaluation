namespace Evaluation.SharedHelper.Models.Api.DepartmentHolidaysDto;

public class CreateDepartmentHolidayDto
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsCronExpression { get; set; }
    public string? CronExpression { get; set; }
    public bool IsActive { get; set; }
}
