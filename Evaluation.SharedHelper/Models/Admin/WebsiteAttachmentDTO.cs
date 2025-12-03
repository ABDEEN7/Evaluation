

namespace Evaluation.SharedHelper.Models.Admin
{
   public class WebsiteAttachmentDTO
    {
        public Guid? RefId { get; set; }
        public string FileName { get; set; } = null!;
        public string UiFileName { get; set; } = null!;
        public string? BlobUrl { get; set; }
        public string FileExtension { get; set; } = null!;
        public long FileSize { get; set; }
        public string? Note { get; set; }
        public string ControlFileName { get; set; } = null!;
    }
}
