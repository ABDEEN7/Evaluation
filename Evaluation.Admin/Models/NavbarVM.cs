


using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models
{
    public class NavbarVM : BaseVM
    {
        public NavbarVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }

        public string Id { get; set; } = null!;
        public List<NavbarDTO> Navbar { get; set; } = new();
      
    }
}
