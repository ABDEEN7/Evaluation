
using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.ServiceEnities;

namespace Evaluation.DAL.Models.Attachments
{
    public class PlaceHolder : EntityBase, IAuditLogEntity
    {
        public Guid ServiceId { get; set; }
        public Service? Service { get; set; }
        public string? PlaceHolderName { get; set; }
        public string? TypeDisplay { get; set; }
        public Guid?  FieldId { get; set; }
        public string?  ChildFieldIds { get; set; }
        public string? Type { get; set; }
        public string? ColumnName { get; set; }
    }
}
