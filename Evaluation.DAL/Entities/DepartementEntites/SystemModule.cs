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
    public class SystemModule : BaseEntities, IAuditLogEntity, ILocalized
    {
        public Guid DepartmentId { get; set; }
        public Department? Department { get; set; }
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string BackendName { get; set; } = null!;
        public string DescriptionAr { get; set; } = null!;
        public string DescriptionEn { get; set; } = null!;
        public string Routing { get; set; } = null!;
        public string Icon { get; set; } = null!;
        public string SchNoDefinition { get; set; } = null!;
        public string? ButtonAr { get; set; }
        public string? ButtonEn { get; set; }

        public int OrderNo { get; set; }
      
        public ICollection<Service> Services { get; set; } = new List<Service>();
       

    }
}
