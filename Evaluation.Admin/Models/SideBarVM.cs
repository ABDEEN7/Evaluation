


using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models
{
    public class SideBarVM : BaseVM
    {
        public SideBarVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }


        public string Lang { get; set; } = null!;

        public List<SideBarDTO> SideBar { get; set; } = new();

    }
}
