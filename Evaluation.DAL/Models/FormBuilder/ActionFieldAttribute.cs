using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;

namespace Evaluation.DAL.Models.FormBuilder
{
    public class ActionFieldAttribute : EntityBase, IAuditLogEntity
    {
        public Guid FieldId { get; set; }
        public Field? Field { get; set; }
        public Guid? SubFieldId { get; set; }
        public Field? SubField { get; set; }
		public Guid? ActionFieldId { get; set; }
		public ActionField? ActionField { get; set; }
		public string AttributeKey { get; set; } = null!;
        public string? AttributeValue { get; set; }
        public string? MessageAr { get; set; }
        public string? MessageEn { get; set; }
        public string? Description { get; set; }
      

    }
}
