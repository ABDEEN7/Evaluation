namespace Evaluation.DAL.Dtos;

public class DepartmentHolidayDto
{
    public Guid Id { get; set; }
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsCronExpression { get; set; }
    public bool IsActive { get; set; }
    public string? CronExpression { get; set; } = string.Empty;
}


