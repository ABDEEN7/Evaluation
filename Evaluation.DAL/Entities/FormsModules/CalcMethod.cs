using Evaluation.DAL.Entities.Audit;
using  Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.DepartementEntites;
using Evaluation.DAL.Entities.Planing;

namespace Evaluation.DAL.Entities.FormsModules;

public class CalcMethod : EntityBase, IAuditLogEntity
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public decimal MinPercentage { get; set; }
    public decimal MaxPercentage { get; set; }
    public decimal MinWeight { get; set; }
    public decimal MaxWeight { get; set; }
    public Guid DepartmentId { get; set; }
    public Department? Department { get; set; }
}
