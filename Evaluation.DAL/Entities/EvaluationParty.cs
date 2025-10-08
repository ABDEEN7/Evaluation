using Evaluation.DAL.Entities.AdminPanel;

namespace Evaluation.DAL.Entities;

public class EvaluationParty : BaseEntities
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public int OrderNo { get; set; }
    public Guid DepartmentId { get; set; }
}
