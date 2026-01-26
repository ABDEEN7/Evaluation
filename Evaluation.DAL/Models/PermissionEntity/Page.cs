using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.DAL.Models.PermissionEntity;

[Index(nameof(BackendName), IsUnique = true)]
public class Page : EntityBase, IAuditLogEntity
{
    public string? Name { get; set; }
    public string BackendName { get; set; } = null!;
}
