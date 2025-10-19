using Evaluation.DAL.Entities.Audit;
using  Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Authentication;
using Evaluation.DAL.Entities.StatusEntities;

namespace Evaluation.DAL.Entities.FormBuilder
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
