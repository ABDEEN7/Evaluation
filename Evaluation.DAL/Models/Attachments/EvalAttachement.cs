using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.FormBuilder;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Models.SystemLog;

namespace Evaluation.DAL.Models.Attachments
{
    public class Attachment : EntityBase, IAuditLogEntity
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
		public Guid? EvaluationRequestId { get; set; }
		public EvaluationRequest? EvaluationRequest { get; set; }
	}
}
