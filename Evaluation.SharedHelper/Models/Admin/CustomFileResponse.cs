namespace Evaluation.SharedHelper.Models.Admin
{
    public class CustomFileResponse
    {
        public List<AttachmentDTO>? Data { get; set; }
        public string? ResponseMessage { get; set; }
        public bool? ResponseStatus { get; set; }
    }
}
