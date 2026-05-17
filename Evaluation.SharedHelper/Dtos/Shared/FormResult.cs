namespace Evaluation.SharedHelper.Dtos.Shared;

public class CalculationFormResult
{
    public string? Name { get; set; }
    public decimal Value { get; set; }
    public int NextEvalDays { get; set; }
    public Guid Id { get; set; }

}
