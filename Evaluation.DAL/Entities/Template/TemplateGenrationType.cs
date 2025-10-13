using Evaluation.DAL.Entities.BaseModule;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.DAL.Entities.Template;
[Index(nameof(BackendName), IsUnique = true)]
public class TemplateGenrationType : BaseEntities
{
    public string TitleAr { get; set; } = null!;
    public string TitleEn { get; set; } = null!;
    public string BackendName { get; set; } = null!;
}
