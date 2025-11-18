using Evaluation.DAL.Models.BaseModule;

namespace Evaluation.DAL.Models.Template;

public class EmailTemplateDocument : EntityBase
{
    public Guid EmailTemplateId { get; set; }
    public EmailTemplate? EmailTemplate { get; set; }
    public Guid TemplateDocId { get; set; }
    public TemplateDocument? TemplateDocument { get; set; }
}
