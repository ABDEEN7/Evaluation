using Evaluation.DAL.Entities.Base;

namespace Evaluation.DAL.Entities.PermissionEntity;

// if this change module i suggset to be ModulePermission
public class PagePermission : EntityBase
{
    public Guid PageId { get; set; }
    public Page? Page { get; set; }

    public Guid PermissionId { get; set; }
    public Permission? Permission { get; set; }
}