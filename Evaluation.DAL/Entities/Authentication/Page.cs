using Evaluation.DAL.Entities.BaseModule;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.DAL.Entities.Authentication;
[Index(nameof(BackendName), IsUnique = true)]
public class Page : EntityBase
{
    public string? Name { get; set; }
    public string BackendName { get; set; } = null!;
}
