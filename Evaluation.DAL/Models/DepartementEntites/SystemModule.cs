using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.Localized;
using Service = Evaluation.DAL.Models.ServiceEnities.Service;

namespace Evaluation.DAL.Models.DepartementEntites
{
    public class SystemModule : EntityBase, IAuditLogEntity, ILocalized
    {
        public Guid DepartmentId { get; set; }
        public Department? Department { get; set; }
        public Guid? SystemModuleTypeId { get; set; }
        public SystemModuleType? SystemModuleType { get; set; }
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string BackendName { get; set; } = null!;
        public string DescriptionAr { get; set; } = null!;
        public string DescriptionEn { get; set; } = null!;
        public string Routing { get; set; } = null!;
        public string Icon { get; set; } = null!;
        public string NoDefinition { get; set; } = null!;
        public string? ButtonAr { get; set; }
        public string? ButtonEn { get; set; }
        public int OrderNo { get; set; }
      
        public virtual ICollection<Service>? Services { get; set; } = new List<Service>();
       

    }
}
