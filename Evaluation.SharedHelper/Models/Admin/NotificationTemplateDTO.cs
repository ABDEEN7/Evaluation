
namespace Evaluation.SharedHelper.Models.Admin
{
    public class NotificationTemplateDTO : EntityBaseDTO
    {
        public string? SubjectAr { get; set; }
        public string? SubjectEn { get; set; }
        public string BodyAr { get; set; } = "";
        public string BodyEn { get; set; } = "";
        public string BackendName { get; set; } = "";
        public Guid? SystemModuleId { get; set; }
        public string? SystemModule { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? ServiceId { get; set; }

    }
}
