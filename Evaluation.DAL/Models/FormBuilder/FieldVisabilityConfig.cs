using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.StatusEntities;
using Evaluation.DAL.Models.UserEntiy;

namespace Evaluation.DAL.Models.FormBuilder
{
    public class FieldVisabilityConfig : EntityBase, IAuditLogEntity
    {
        public Guid PartyTypeId { get; set; }
        public PartyType? PartyType { get; set; }
        public Guid FieldId { get; set; }
        public Field? Field { get; set; }
        public Guid ServiceStatusId { get; set; }
        public ServiceStatus? ServiceStatus { get; set; }
        public bool IsShow { get; set; }
    }
}
