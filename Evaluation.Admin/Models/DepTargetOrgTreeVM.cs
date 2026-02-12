using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models
{
    public class DepTargetOrgTreeVM : BaseVM
    {
        public DepTargetOrgTreeVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }

        public string Id { get; set; } = null!;
        public List<DepTargetOrgTreeDTO> DepTargetOrgTree { get; set; } = new();
      
    }
}
