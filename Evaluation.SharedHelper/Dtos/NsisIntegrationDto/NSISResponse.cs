using Newtonsoft.Json;

namespace Evaluation.SharedHelper.Dtos.NsisIntegrationDto;

public class NSISSchoolsResponse
{
    [JsonProperty("$id")]

    public int Id { get; set; }
    public List<SchoolDto> Orgs { get; set; }
}
public class NSISSchoolResponse
{
    [JsonProperty("$id")]

    public int Id { get; set; }
    public SchoolDto Org { get; set; }
}
