using Evaluation.DAL.Models.BaseModule;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.DAL.Models.Calendars;
[Index(nameof(BackendName), IsUnique = true)]
public class PlanType : EntityBase
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public int OrderNo { get; set; }
    public string BackendName { get; set; } = null!;
}
