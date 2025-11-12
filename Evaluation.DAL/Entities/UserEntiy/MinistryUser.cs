using  Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Masters;
using Evaluation.DAL.Entities.UserEntiy;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.DAL.Entities.Authentication;
[Index(nameof(QID), IsUnique = true)]

public class MinistryUser : EntityBase
{
    public string QID { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? NationalityCode { get; set; }
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
	public Guid UserGenderId { get; set; }
	public UserGender? UserGender { get; set; }
	public DateTime? LastLoginDate { get; set; }
    public string PreferredLanguage { get; set; } = null!;
    public string? Mobile { get; set; }
    public string JobTitleEn { get; set; } = null!;
    public string JobTitleAr { get; set; } = null!;
    public string JobTitleCode { get; set; } = string.Empty;
    public string DirectManagerQId { get; set; } = string.Empty;
    public int EmployeeNo { get; set; }
    public int OrganizationNo { get; set; }
    public ICollection<UserPartyType>? UserPartTypes { get; set; } = new List<UserPartyType>();
    public ICollection<UserRole>? UserRoles { get; set; } = new List<UserRole>();
}
