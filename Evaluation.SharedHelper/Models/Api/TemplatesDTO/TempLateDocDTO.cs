
using Evaluation.SharedHelper.Models.Api.FormBuilderDTO;

namespace Evaluation.SharedHelper.Models.Api.TemplatesDTO
{
    public class TempLateDocDTO:EntityBaseDTO
    {
        public string NameEn { get; set; } = null!;
        public string NameAr { get; set; } = null!;
        public string? TemplateAr { get; set; }
        public string? TemplateEn { get; set; }
        public Guid? AttachmentId { get; set; }
        public bool? IsAttachment { get; set; }
        public Guid? SystemModuletId { get; set; }
        public string? SystemModule { get; set; }
        public Guid? ServiceId { get; set; }
        public string? Service { get; set; }
        public Guid TemplateGenrationTypeId { get; set; }
        public string TemplateGenrationType { get; set; } = null!;
        public string? Name { get; set; }
        public Guid? TemplateConfiguration { get; set; }
        public string ActionBackEndName { get; set; } = null!;
        public Guid? FieldId { get; set; }
        public FieldDTO? Field { get; set; }
        public Guid? SubFieldId { get; set; }
        public FieldDTO? SubField { get; set; }
    }
}
