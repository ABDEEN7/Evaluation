
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Evaluation.SharedHelper.Dtos.NsisIntegrationDto;

public class SchoolParentDto
{
    [JsonPropertyName("$id")]
    public string? Id { get; set; }
    public Guid SourcedId { get; set; }
    public string? Type { get; set; }
    public string? Href { get; set; }
}
