using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models
{
    public class PlanTypeDepVM : BaseVM
    {
        public PlanTypeDepVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }

        public string Id { get; set; } = null!;
        public List<PlanTypeDepDTO> PlanTypeDep { get; set; } = new();
      
    }
}
