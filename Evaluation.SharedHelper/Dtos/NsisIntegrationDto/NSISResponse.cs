using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Evaluation.SharedHelper.Dtos.NsisIntegrationDto;

public class NSISSchoolsResponse
{
    [JsonPropertyName("$id")]
    public string? Id { get; set; }
    public List<SchoolDto> Orgs { get; set; }
}
public class NSISSchoolResponse
{
    [JsonPropertyName("$id")]
    public string? Id { get; set; }
    public SchoolDto Org { get; set; }
}
public class NSISClassResponse
{
    [JsonPropertyName("$id")]
    public string? Id { get; set; }
    public List<Class> Classes { get; set; }
}
