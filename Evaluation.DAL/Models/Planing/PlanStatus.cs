using Evaluation.DAL.Models.BaseModule;

namespace Evaluation.DAL.Models.Planing;

public class PlanStatus : EntityBase
{
    public string NameAr { get; set; } = null!;
    public string NameEN { get; set; } = null!;
    public string BackendName { get; set; } = null!;
    public int OrderNo { get; set; }
}
