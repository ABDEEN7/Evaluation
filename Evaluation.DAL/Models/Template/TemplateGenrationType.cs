using Evaluation.DAL.Models.BaseModule;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.DAL.Models.Template;
[Index(nameof(BackendName), IsUnique = true)]
public class TemplateGenrationType : EntityBase
{
    public string TitleAr { get; set; } = null!;
    public string TitleEn { get; set; } = null!;
    public string BackendName { get; set; } = null!;
}
