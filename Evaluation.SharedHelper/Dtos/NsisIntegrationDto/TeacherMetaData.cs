using Newtonsoft.Json;

namespace Evaluation.SharedHelper.Dtos.NsisIntegrationDto;  

public class TeacherMetaData
{
    [JsonProperty("$id")]
    public string? Id { get; set; }
    public string? EnglishName { get; set; }
    public List<string?> Address { get; set; }
}
