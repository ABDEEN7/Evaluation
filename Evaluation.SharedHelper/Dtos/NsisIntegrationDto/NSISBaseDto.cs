
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Evaluation.SharedHelper.Dtos.NsisIntegrationDto;

public abstract class NSISBaseDto
{
    [JsonPropertyName("$id")]
    public string? Id { get; set; }
    public Guid sourcedId { get; set; }
    public string? Status { get; set; }
    public string? DateLastModified { get; set; }
}
