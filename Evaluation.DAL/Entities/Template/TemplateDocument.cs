using Evaluation.DAL.Entities.Audit;
using Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.ServicesEntities;
using Evaluation.DAL.Entities.SystemModulesEntities;

namespace Evaluation.DAL.Entities.Template;

public class TemplateDocument : EntityBase, IAuditLogEntity
{
	public string NameEn { get; set; } = null!;
	public string NameAr { get; set; } = null!;
	public string? TemplateAr { get; set; }
	public string? TemplateEn { get; set; }
	public Guid? AttachmentId { get; set; }
	public bool? IsAttachment { get; set; }
	public Guid SystemModuletId { get; set; }
	public SystemModule? SystemModule { get; set; }
	public Guid ServiceId { get; set; }
	public Service? Service { get; set; }
	public Guid TemplateGenrationTypeId { get; set; }
	public TemplateGenrationType? TemplateGenrationType { get; set; }

}
