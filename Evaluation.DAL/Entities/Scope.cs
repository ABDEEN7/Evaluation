using Evaluation.DAL.Entities.AdminPanel;
using Evaluation.DAL.Entities.Planing;

namespace Evaluation.DAL.Entities;

public class Scope : BaseEntities
{
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public Guid DepartmentId { get; set; }
    public Guid ParentId { get; set; }
    public Department Department { get; set; }
    public Scope Parent { get; set; }
}
