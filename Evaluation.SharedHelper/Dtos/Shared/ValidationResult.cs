namespace Evaluation.SharedHelper.Dtos.Shared;

public class ValidationResult
{
    public bool IsValid => !Errors.Any();
    public List<ItemError> Errors { get; } = new();
}