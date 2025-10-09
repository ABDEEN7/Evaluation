using Evaluation.DAL.Entities.AdminPanel;

namespace Evaluation.DAL.Entities.Template;

public class TemplateGenrationType : BaseEntities
{
    public string TitleAr { get; set; } = null!;
    public string TitleEn { get; set; } = null!;
    public string BackendName { get; set; } = null!;
}
