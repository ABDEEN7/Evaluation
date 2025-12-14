using Evaluation.DAL.Models.BaseModule;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.DAL.Models.Template;
[Index(nameof(BackendName), IsUnique = true)]
public class StatusService : EntityBase
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public string BackendName { get; set; }
}
