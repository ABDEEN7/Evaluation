using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.DepartementEntites;

namespace Evaluation.DAL.Models.FormsModules;

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
