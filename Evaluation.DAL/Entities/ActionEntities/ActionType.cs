using Microsoft.EntityFrameworkCore;
using Evaluation.DAL.Entities.Audit;
using Evaluation.DAL.Entities.Base;

namespace Evaluation.DAL.Entities.ActionEntities
{
    [Index(nameof(BackendName), IsUnique = true)]

    public class ActionType : BaseEntities, IAuditLogEntity
    {
        public string TitleAr { get; set; } = null!;
        public string TitleEn { get; set; } = null!;
        public string BackendName { get; set; } = null!;
        public int? OrderNo { get; set; }
        public bool? IsRequiredValidation { get; set; }
        public virtual ICollection<ServiceAction>? Actions { get; set; }
    }
}
