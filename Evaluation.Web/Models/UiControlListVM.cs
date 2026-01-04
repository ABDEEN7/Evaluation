

using Evaluation.SharedHelper.Models.Api;

namespace Evaluation.Web.Models
{
    public class UiControlListVM 
    {
        public List<UiControlItemDTO> UiControlItemList { get; set; } = new();
        public string Lang { get; set; } = null!;

    }
}
