using Evaluation.DAL.Entities.Audit;
using  Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.FormBuilder;
using Evaluation.DAL.Entities.ServicesEntities;

namespace Evaluation.DAL.Entities.Attachments
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
