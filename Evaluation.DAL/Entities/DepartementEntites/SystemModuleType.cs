using Evaluation.DAL.Entities.ActionEntities;
using Evaluation.DAL.Entities.Audit;
using Evaluation.DAL.Entities.Base;
using Evaluation.DAL.Entities.FormBuilder;
using Evaluation.DAL.Entities.Authentication;
using Evaluation.DAL.Entities.ServicesEntities;
using Evaluation.DAL.Entities.Template;
using Evaluation.DAL.Entities.DepartementEntites;

namespace Evaluation.DAL.Entities.SystemModulesEntities
{
    public class SystemModuleType : BaseEntities, IAuditLogEntity, ILocalized
    {
       
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string BackendName { get; set; } = null!;

        public Guid? ParentModuleTypeId { get; set; }
        public SystemModuleType? ParentModuleType { get; set; }


    }
}
