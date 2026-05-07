using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.Authentication
{
	public class UserProfileDTO : EntityBaseDTO
	{
		public string Email { get; set; } = null!;
		public string QID { get; set; } = null!;
		public string FullNameEn { get; set; } = null!;
		public string FullNameAr { get; set; } = null!;
		public DateTime? LastLoginDate { get; set; }
		public string Mobile { get; set; } = null!;
		public string PrefferedLang { get; set; } = null!;
		public string JobDescription { get; set; } = null!;
		public string? NationalityCode { get; set; }
		public string? Type { get; set; }
		public Guid RoleId { get; set; }
		public string Role { get; set; } = null!;
		public List<PermissionDTO>? PermissionList { get; set; }
		public string? ProfilePhoto { get; set; }
		public string? SecondMobile { get; set; }
		public bool IsVerifiedSecondMobile { get; set; }
	}
}
