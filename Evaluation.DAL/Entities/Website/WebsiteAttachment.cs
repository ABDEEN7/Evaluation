using Evaluation.DAL.Entities.Audit;
using Evaluation.DAL.Entities.BaseModule;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Entities.Website;

   public class WebsiteAttachment : EntityBase, IAuditLogEntity
    {
        public Guid? RefId { get; set; }
        public string FileName { get; set; } = null!;
        public string UiFileName { get; set; } = null!;
        public string? BlobUrl { get; set; }
        public string FileExtension { get; set; } = null!;
        public long FileSize { get; set; }
        public string? Note { get; set; }
    }
