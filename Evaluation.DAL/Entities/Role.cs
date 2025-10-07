using Evaluation.DAL.Entities.AdminPanel;

namespace Evaluation.DAL.Entities;

public class Role : BaseEntities
{
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public List<UserRole> UserRoles { get; set; } = new();
}
