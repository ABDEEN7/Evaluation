

namespace Evaluation.SharedHelper.Models.Admin
{
    public class SMSTemplateDTO:EntityBaseDTO
    {
     

        public string TitleAr { get; set; } = null!;
        public string TitleEn { get; set; } = null!;
        public string Messages { get; set; } = null!;
        public string BackendName { get; set; } = null!;

        public Guid SMSProfileId { get; set; }

        public string SMSProfile { get; set; } = null!;
        public Guid? DepartmentId { get; set; } 
        public Guid? ServiceId { get; set; } 
        public string? Department { get; set; } 
        public string? Service { get; set; } 
    }
}
