
using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models
{
    public class PermissionVM : BaseVM
    {
        public PermissionVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }

        public string Id { get; set; } = null!;
        public List<RolePermissionDTO> RolePermission { get; set; } = new();
      
    }
}
