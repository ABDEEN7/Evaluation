namespace Evaluation.SharedHelper.Models;

public class ValidationResult
{
    public bool IsValid => !Errors.Any();
    public List<string> Errors { get; } = new();

    public void Add(string error)
    {
        Errors.Add(error);
    }
}