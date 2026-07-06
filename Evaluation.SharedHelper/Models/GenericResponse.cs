using Evaluation.SharedHelper.Enums;

namespace Evaluation.SharedHelper.Models;

public class GenericResponse
{
    public DBResult ResponseStatus { get; set; }
    public string? ResponseMessage { get; set; }
    public bool? ResponseState { get; set; }
}
