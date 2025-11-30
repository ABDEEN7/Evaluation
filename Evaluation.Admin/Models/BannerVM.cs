


using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models
{
    public class BannerVM : BaseVM
    {
        public BannerVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }

        public string Id { get; set; } = null!;

        public List<BannerDTO> Banner { get; set; } = new();
      
    }
}
