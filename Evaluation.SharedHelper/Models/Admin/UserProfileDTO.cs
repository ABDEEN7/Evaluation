

namespace Evaluation.SharedHelper.Models.Admin
{
    public class UserProfileDTO : EntityBaseDTO
    {
        public string QID { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? NationalityCode { get; set; }
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public Guid UserGenderId { get; set; }

        public string? UserGender { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string PreferredLanguage { get; set; } = null!;
        public string? Mobile { get; set; }
        public string JobTitleEn { get; set; } = null!;
        public string JobTitleAr { get; set; } = null!;
        public string JobTitleCode { get; set; } = string.Empty;
        public string DirectManagerQId { get; set; } = string.Empty;
        public string? EmployeeNo { get; set; }
        public string? OrganizationNo { get; set; }
        public Guid RoleId { get; set; }
        public string Role { get; set; } = null!;

    }
}

