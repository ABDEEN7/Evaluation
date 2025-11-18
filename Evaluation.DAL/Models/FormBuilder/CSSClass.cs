using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;

namespace Evaluation.DAL.Models.FormBuilder
{
    public class CssClass : EntityBase, IAuditLogEntity
    {
        public string ClassName { get; set; } = null!;
        public string Styles { get; set; } = null!;
        public Guid ApplyTypeId { get; set; }
        public CssApplyType? ApplyType { get; set; }
    }
}
