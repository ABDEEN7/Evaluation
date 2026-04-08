using Newtonsoft.Json;

namespace Evaluation.SharedHelper.Dtos.NsisIntegrationDto;

public class StaffMetaData
{
    [JsonProperty("$id")]
    public string? Id { get; set; }
    public Guid? JobFunctionID { get; set; }
    public string? JobFunction { get; set; }
    public string? ArabicJobFunction { get; set; }
    public Guid? TeachingAssignmentID { get; set; }
    public string? TeachingAssignment { get; set; }
    public string? ArabicTeachingAssignment { get; set; }
    public string? EnglishName { get; set; }
    public string? Address { get; set; }
}
