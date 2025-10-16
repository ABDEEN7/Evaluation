using Evaluation.DAL.Entities.Audit;
using Evaluation.DAL.Entities.Base;

namespace Evaluation.DAL.Entities.FormBuilder
{
    public class CssClass : BaseEntities, IAuditLogEntity
    {
        public string ClassName { get; set; } = null!;
        public string Styles { get; set; } = null!;
        public Guid ApplyTypeId { get; set; }
        public CssApplyType? ApplyType { get; set; }
    }
}
