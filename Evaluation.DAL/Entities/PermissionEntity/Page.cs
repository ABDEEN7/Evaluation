using Evaluation.DAL.Entities.Base;

namespace Evaluation.DAL.Entities.PermissionEntity;

public class Page : BaseEntities
{
    public string? Name { get; set; }
    public string BackendName { get; set; } = null!;
}
