


using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models
{
    public class DropDownTypeVM : BaseVM
    {
        public DropDownTypeVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }

        public string Id { get; set; } = null!;
        public List<DropDownTypeDTO> DropDownType { get; set; } = new();
      
    }
}
