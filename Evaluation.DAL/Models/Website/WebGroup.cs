using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;


namespace Evaluation.DAL.Models.Website
{
    public class WebGroup : EntityBase, IAuditLogEntity
    {
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string RoutingPath { get; set; } = null!;
        public string BackendName { get; set; } = null!;
    }
}
