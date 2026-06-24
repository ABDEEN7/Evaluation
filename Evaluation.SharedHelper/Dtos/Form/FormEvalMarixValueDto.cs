namespace Evaluation.SharedHelper.Dtos.Form;

public class FormEvalMarixValueDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal MaxValue { get; set; }
    public decimal MinValue { get; set; }
    public decimal ActualMatrixValue { get; set; }
    public string DisplayRange { get; set; } = null!;
}
