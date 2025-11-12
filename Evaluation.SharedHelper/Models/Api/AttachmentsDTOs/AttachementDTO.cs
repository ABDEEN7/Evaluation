using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.AttachmentsDTOs
{
    public class AttachementDTO : EntityBaseDTO
    {
        public string? ActionTransationLogId { get; set; }
        public string? FieldId { get; set; }
        public string? ServiceRequestId { get; set; }
        public string FileName { get; set; } = null!;
        public string UiFileName { get; set; } = null!;
        public string? BlobUrl { get; set; }
        public string FileExtension { get; set; } = null!;
        public long FileSize { get; set; }
        public bool IsOthers { get; set; }
    }
}
