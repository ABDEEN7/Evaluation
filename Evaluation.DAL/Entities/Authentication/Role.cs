using Evaluation.DAL.Entities.BaseModule;

namespace Evaluation.DAL.Entities.Authentication;

public class Role : BaseEntities
{
    public required string NameAr { get; set; }
    public required string NameEn { get; set; }
    public List<UserRole> UserRoles { get; set; } = new();
}
