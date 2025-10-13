using Evaluation.DAL.Entities.BaseModule;

namespace Evaluation.DAL.Entities.Template;

public class EmailTemplateDocument : BaseEntities
{
    public Guid EmailTemplateId { get; set; }
    public EmailTemplate? EmailTemplate { get; set; }
    public Guid TemplateDocId { get; set; }
    public TemplateDocument? TemplateDocument { get; set; }
}
