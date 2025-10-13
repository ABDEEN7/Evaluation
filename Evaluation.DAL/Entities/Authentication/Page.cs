using Evaluation.DAL.Entities.AdminPanel;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.DAL.Entities.Authentication;
[Index(nameof(BackendName), IsUnique = true)]
public class Page : BaseEntities
{
    public string? Name { get; set; }
    public string BackendName { get; set; } = null!;
}
