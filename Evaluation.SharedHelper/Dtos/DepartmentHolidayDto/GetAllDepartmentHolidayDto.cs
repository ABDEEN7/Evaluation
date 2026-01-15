namespace Evaluation.SharedHelper.Dtos.DepartmentHolidayDto;

public class GetAllDepartmentHolidayDto
{
    public string NameEn { get; set; }
    public string NameAr { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsCronExpression { get; set; }
    public string? CronExpression { get; set; }
}
