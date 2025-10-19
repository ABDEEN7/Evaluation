using Evaluation.DAL.Entities.Audit;
using  Evaluation.DAL.Entities.BaseModule;

namespace Evaluation.DAL.Entities.FormBuilder
{
    public class CssClass : EntityBase, IAuditLogEntity
    {
        public string ClassName { get; set; } = null!;
        public string Styles { get; set; } = null!;
        public Guid ApplyTypeId { get; set; }
        public CssApplyType? ApplyType { get; set; }
    }
}
