using Evaluation.DAL.Entities.AdminPanel;

namespace Evaluation.DAL.Entities.Calendars;

public class PlanType : BaseEntities
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public int OrderNo { get; set; }
    public string BackendName { get; set; }
}
