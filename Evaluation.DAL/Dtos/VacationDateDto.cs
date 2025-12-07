namespace Evaluation.DAL.Dtos;

public class VacationDateDto
{
    public Guid Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsCronExpression { get; set; }
    public string CronExpression { get; set; } = string.Empty;
}
