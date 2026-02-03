using Newtonsoft.Json;

namespace Evaluation.SharedHelper.Dtos.NsisIntegrationDto;  

public class TeacherMetaData
{
    [JsonProperty("$id")]
    public string? Id { get; set; }
    public string? EnglishName { get; set; }
    public string? Address { get; set; }
}
