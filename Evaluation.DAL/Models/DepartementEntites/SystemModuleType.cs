using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.Localized;

namespace Evaluation.DAL.Models.DepartementEntites
{
    public class SystemModuleType : EntityBase, IAuditLogEntity, ILocalized
    {
       
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string BackendName { get; set; } = null!;

        public Guid? ParentModuleTypeId { get; set; }
        public SystemModuleType? ParentModuleType { get; set; }


    }
}
