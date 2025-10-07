using Evaluation.DAL.Entities.AdminPanel;

namespace Evaluation.DAL.Entities;

public class Scope : BaseEntities
{
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public Guid DepartmentId { get; set; }
    public Guid ParentId { get; set; }
}
