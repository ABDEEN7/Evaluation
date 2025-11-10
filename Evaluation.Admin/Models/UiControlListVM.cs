


using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models
{
    public class UiControlListVM : BaseVM
    {
        public UiControlListVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }

        public List<UiControlItemDTO> UiControlItemList { get; set; } = new();
        public string Lang { get; set; } = null!;

    }
}
