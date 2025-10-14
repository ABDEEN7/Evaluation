using Evaluation.DAL.Entities.BaseModule;

namespace Evaluation.DAL.Entities.Authentication;

// if this change module i suggset to be ModulePermission
public class PagePermission : EntityBase
{
    public Guid PageId { get; set; }
    public Page? Page { get; set; }

    public Guid PermissionId { get; set; }
    public Permission? Permission { get; set; }
}