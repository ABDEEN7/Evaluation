using Evaluation.DAL.Entities.BaseModule;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.DAL.Entities.Calendars;
[Index(nameof(BackendName), IsUnique = true)]
public class PlanType : BaseEntities
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public int OrderNo { get; set; }
    public string BackendName { get; set; } = null!;
}
