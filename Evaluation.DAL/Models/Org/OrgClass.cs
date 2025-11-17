using Evaluation.DAL.Models.BaseModule;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.DAL.Models.Org;
[Index(nameof(BackendName), IsUnique = true)]
public class OrgClass : EntityBase
{
    public Guid ParentId { get; set; }
    public OrgClass? Parent { get; set; }
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public string BackendName { get; set; } = null!;
    public string? HRCode { get; set; }
}
