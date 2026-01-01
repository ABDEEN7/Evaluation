using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.DepartementEntites;

namespace Evaluation.DAL.Models.Planing;

public class DepartmentEvaluationParty : EntityBase
{
    public Guid DepartmentId { get; set; }
    public Department? Department { get; set; }
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public int OrderNo { get; set; } = 0;
}
