using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models
{
    public class RoleVM : BaseVM
    {
        public RoleVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }

        public string Id { get; set; } = null!;
        public List<RoleDTO> Role { get; set; } = new();
      
    }
}
