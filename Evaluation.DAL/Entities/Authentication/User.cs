using Evaluation.DAL.Entities.AdminPanel;

namespace Evaluation.DAL.Entities.Authentication;

public class User : BaseEntities
{
    public string QID { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? NationalityCode { get; set; }
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public DateTime? LastLoginDate { get; set; }
    public string PreferredLanguage { get; set; } = null!;
    public string? Mobile { get; set; }
    public  ICollection<UserPartyType>? UserPartTypes { get; set; } = new List<UserPartyType>();
    public  ICollection<UserRole>? UserRoles { get; set; } = new List<UserRole>();
}
