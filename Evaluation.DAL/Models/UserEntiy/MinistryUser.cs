using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.Master;
using Evaluation.DAL.Models.Planing.TeamsModule;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evaluation.DAL.Models.UserEntiy;
[Index(nameof(QID), IsUnique = true)]

public class MinistryUser : EntityBase
{
    public string QID { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? NationalityCode { get; set; }
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    [NotMapped]
	public Guid UserGenderId { get; set; }
	[NotMapped]

	public UserGender? UserGender { get; set; }
	public DateTime? LastLoginDate { get; set; }
    public string PreferredLanguage { get; set; } = null!;
    public string? Mobile { get; set; }
    public string JobTitleEn { get; set; } = null!;
    public string JobTitleAr { get; set; } = null!;
    public string JobTitleCode { get; set; } = string.Empty;
    public string DirectManagerQId { get; set; } = string.Empty;
    public string? EmployeeNo { get; set; }
    public string? OrganizationNo { get; set; }
    public ICollection<UserPartyType>? UserPartTypes { get; set; }
    public ICollection<UserRole>? UserRoles { get; set; } 
    public ICollection<UserTeam>? UserTeams { get; set; }
}
