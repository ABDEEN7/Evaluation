using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models
{
    public class PlanStatusesVM : BaseVM
    {
        public PlanStatusesVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }

        public string Id { get; set; } = null!;
        public List<PlanStatusesDTO> PlanStatuses { get; set; } = new();
      
    }
}
