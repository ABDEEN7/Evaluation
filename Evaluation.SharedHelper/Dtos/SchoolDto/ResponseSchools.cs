namespace Evaluation.SharedHelper.Dtos.SchoolDto;

public class ResponseSchools
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime? LastEvaluationDate { get; set; }
    public int GradLevel { get; set; }
}
