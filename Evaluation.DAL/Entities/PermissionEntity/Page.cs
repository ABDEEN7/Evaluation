using Evaluation.DAL.Entities.Base;

namespace Evaluation.DAL.Entities.PermissionEntity;

public class Page : BaseEntities
namespace Evaluation.DAL.Entities.Authentication;
[Index(nameof(BackendName), IsUnique = true)]
public class Page : EntityBase
{
    public string? Name { get; set; }
    public string BackendName { get; set; } = null!;
}
