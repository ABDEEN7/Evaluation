using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.DepartementEntites;

namespace Evaluation.DAL.Models.FormsModules;

public class Scope : EntityBase, IAuditLogEntity
{
    public string? ScopeNumber { get; set; }
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public Guid ScopeTypeId { get; set; }
    public ScopeType? ScopeType { get; set; }
    public string? ColorCode { get; set; }
    public int OrderNo { get; set; }
}