

namespace Evaluation.SharedHelper.Models.Admin
{
    public class EmailTemplateDTO:EntityBaseDTO
    {
        public string TitleAr { get; set; } = null!;
        public string TitleEn { get; set; } = null!;
        public string TemplateSubject { get; set; } = null!;
        public string TemplateBody { get; set; } = null!;
        public string BackendName { get; set; } = null!;
        public Guid EmailProfileId { get; set; }
        public string EmailProfile { get; set; } = null!;
        public Guid? ServiceId { get; set; }
        public string? Service { get; set; }
        public Guid[]? FielsFromRequest { get; set; }
        public Guid[]? FielsFromEvaluation { get; set; }

    }
}
