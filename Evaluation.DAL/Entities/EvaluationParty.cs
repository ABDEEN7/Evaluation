using Evaluation.DAL.Entities.AdminPanel;

namespace Evaluation.DAL.Entities;

public class EvaluationParty : BaseEntities
{
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public int OrderNo { get; set; }
    public Guid DepartmentId { get; set; }
}
