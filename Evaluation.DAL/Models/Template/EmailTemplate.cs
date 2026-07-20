using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.ServiceEnities;
using Evaluation.DAL.Models.SystemSetting;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.DAL.Models.Template;
[Index(nameof(BackendName), IsUnique = true)]
public class EmailTemplate: EntityBase, IAuditLogEntity
{
    public string TitleAr { get; set; } = null!;
    public string TitleEn { get; set; } = null!;
    public string TemplateSubject { get; set; } = null!;
    public string TemplateBody { get; set; } = null!;
    public string BackendName { get; set; } = null!;

    public Guid EmailProfileId { get; set; }
    public EmailProfile? EmailProfile { get; set; }

    public Guid? ServiceId { get; set; }
    public Service? Service { get; set; }

    public string? FielsFromRequest { get; set; }
    public string? FielsFromEvaluation { get; set; }
    public Guid? DepartmentId { get; set; }
    public Department? Department { get; set; }

}
