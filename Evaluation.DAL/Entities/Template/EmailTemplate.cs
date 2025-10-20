using Evaluation.DAL.Entities.BaseModule;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.DAL.Entities.Template;
[Index(nameof(BackendName), IsUnique = true)]
public class EmailTemplate: EntityBase
{
    public string TitleAr { get; set; } = null!;
    public string TitleEn { get; set; } = null!;
    public string TemplateSubject { get; set; } = null!;
    public string TemplateBody { get; set; } = null!;
    public string BackendName { get; set; } = null!;

    public Guid EmailProfileId { get; set; }
    //public EmailProfile? EmailProfile { get; set; }

    public Guid? ServiceId { get; set; }
    //public Service? Service { get; set; }

    public string? FielsFromRequest { get; set; }
    public string? FielsFromEvaluation { get; set; }
}
