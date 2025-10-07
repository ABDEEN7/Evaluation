using Evaluation.DAL.Entities.AdminPanel;

namespace Evaluation.DAL.Entities;

public class Department : BaseEntities
{
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public Guid CategoryId { get; set; }
    public List<Department> Departments { get; set; }
}