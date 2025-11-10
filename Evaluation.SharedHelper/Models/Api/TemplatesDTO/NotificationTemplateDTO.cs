

namespace Evaluation.SharedHelper.Models.Api.TemplatesDTO
{
    public class NotificationTemplateDTO : EntityBaseDTO
    {
        public string? TemplateSubjectAr { get; set; }
        public string? TemplateSubjectEn { get; set; }
        public string TemplateBodyAr { get; set; } = "";
        public string TemplateBodyEn { get; set; } = "";
        public string BackendName { get; set; } = "";
        public Guid? SystemModuleId { get; set; }
        public string? SystemModule { get; set; }
        
    }
}
