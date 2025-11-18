using Microsoft.EntityFrameworkCore;
using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;

namespace Evaluation.DAL.Models.ActionEntities
{
    [Index(nameof(BackendName), IsUnique = true)]

    public class ActionType : EntityBase, IAuditLogEntity
    {
        public string TitleAr { get; set; } = null!;
        public string TitleEn { get; set; } = null!;
        public string BackendName { get; set; } = null!;
        public int? OrderNo { get; set; }
        public bool? IsRequiredValidation { get; set; }
        public virtual ICollection<ServiceAction>? Actions { get; set; }
    }
}
