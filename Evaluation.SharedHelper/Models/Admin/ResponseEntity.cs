using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Evaluation.SharedHelper.Models.Admin
{
    public class ResponseEntity
    {
        public ResponseEntity(object data)
        {
            this.Data = data;
            this.Status = StatusCodes.Status200OK;
        }
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public object? Data { get; set; }
        [JsonProperty("responseState", NullValueHandling = NullValueHandling.Ignore)]
        public int? Status { get; set; }
        [JsonProperty("errors", NullValueHandling = NullValueHandling.Ignore)]
        public IList<string>? Errors { get; set; }
        [JsonProperty("responseMessage", NullValueHandling = NullValueHandling.Ignore)]
        public string? Message { get; set; }
        [JsonProperty("last_page", NullValueHandling = NullValueHandling.Ignore)]
        public long? LastPage { get; set; }
    }
}
