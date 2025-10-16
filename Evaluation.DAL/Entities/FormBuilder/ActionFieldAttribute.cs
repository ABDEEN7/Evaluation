using Evaluation.DAL.Entities.Audit;
using Evaluation.DAL.Entities.Base;
using Evaluation.DAL.Entities.FormBuilder;

namespace Evaluation.DAL.Entities.ActionEntities
{
    public class ActionFieldAttribute : BaseEntities, IAuditLogEntity
    {
        public Guid FieldId { get; set; }
        public Field? Field { get; set; }
        public Guid? SubFieldId { get; set; }
        public Field? SubField { get; set; }
        public string AttributeKey { get; set; } = null!;
        public string? AttributeValue { get; set; }
        public string? MessageAr { get; set; }
        public string? MessageEn { get; set; }
        public string? Description { get; set; }
        public Guid? ActionStepFieldId { get; set; }

    }
}
