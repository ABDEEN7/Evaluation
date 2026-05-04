using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.Planing;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;

namespace Evaluation.DAL.Models.Org;

public class OrgTree : EntityBase , IAuditLogEntity
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public Guid? OrgParentId { get; set; }
    public OrgTree? OrgParent { get; set; }
    public string? HrCode { get; set; } 
    public string? NSISCode { get; set; }
    public Guid OrgTypeId { get; set; }
    public OrgType? OrgType { get; set; }
    public Guid OrgClassId { get; set; }
    public OrgClass? OrgClass { get; set; }
    public virtual ICollection<EvaluationRequest>? EvaluationRequests { get; set; }
}