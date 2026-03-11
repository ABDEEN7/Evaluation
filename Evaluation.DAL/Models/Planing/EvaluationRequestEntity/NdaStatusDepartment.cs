using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.DepartementEntites;

namespace Evaluation.DAL.Models.Planing.EvaluationRequestEntity;

public class NdaStatusDepartment : EntityBase, IAuditLogEntity
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public Guid DepartmentId { get; set; }
    public Department? Department { get; set; }
    public Guid NdaStatusId { get; set; }
    public NdaStatus? NdaStatus { get; set; }
    public int OrderNo { get; set; } = 0;

}
