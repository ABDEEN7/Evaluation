
using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models
{
    public class UserPermissionVM : BaseVM
    {
        public UserPermissionVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }

        public string Id { get; set; } = null!;
        public List<UserProfileDTO> UserProfile { get; set; } = new();
      
    }
}
