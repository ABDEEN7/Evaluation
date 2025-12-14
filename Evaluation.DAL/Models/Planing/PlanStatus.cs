using Evaluation.DAL.Models.BaseModule;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.DAL.Models.Planing;
[Index(nameof(BackendName), IsUnique = true)]
public class PlanStatus : EntityBase
{
    public string NameAr { get; set; } = null!;
    public string NameEN { get; set; } = null!;
    public string BackendName { get; set; } = null!;
    public int OrderNo { get; set; }
}
