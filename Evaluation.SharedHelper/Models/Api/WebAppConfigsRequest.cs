namespace Evaluation.SharedHelper.Models.Api
{
    public class WebAppConfigsRequest
    {
        public List<string>? keys { get; set; }
        public List<string>? pageNames { get; set; }
        public string? lang { get; set; }
        public string? department { get; set; }
    }
}
