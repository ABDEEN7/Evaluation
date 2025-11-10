


namespace Evaluation.SharedHelper.Models.Admin
{
    public class AttachmentDTO
    {
        public string ControlFileName { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public string UiFileName { get; set; } = null!;
        public string? BlobUrl { get; set; }
        public string FileExtension { get; set; } = null!;
        public long FileSize { get; set; }
        
    }
}
