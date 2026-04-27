using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.DepartementEntites;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.DAL.Models.FormsModules;

[Index(nameof(BackendName), IsUnique = true)]
public class CalcMethod : EntityBase, IAuditLogEntity
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public string BackendName { get; set; }=null!;
    public decimal MinPercentage { get; set; }
    public decimal MaxPercentage { get; set; }
    public decimal MinWeight { get; set; }
    public decimal MaxWeight { get; set; }
    public int OrderNo { get; set; } = 0;

}
