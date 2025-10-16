using Evaluation.DAL.Entities.Audit;
using Evaluation.DAL.Entities.Base;
using Evaluation.DAL.Entities.FormBuilder;
using Evaluation.DAL.Entities.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Entities.Attachments
{
    public class Attachment : BaseEntities, IAuditLogEntity
    {
      
        public Guid? ActionTransactionsLogId { get; set; }
        public ActionTransactionsLog? ActionTransactionsLog { get; set; }
        public Guid? FieldId { get; set; }
        public Field? Field { get; set; }
        public Guid? ServiceRequestId { get; set; }
        public string FileName { get; set; } = null!;
        public string UiFileName { get; set; } = null!;
        public string FileExtension { get; set; } = null!;
        public long FileSize { get; set; }
        public bool IsOthers { get; set; }
        public Guid? ChildFieldId { get; set; }
        public Field? ChildField { get; set; }
        public string? Index { get; set; }
    }
}
