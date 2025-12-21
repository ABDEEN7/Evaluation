

using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models
{
    public class OrgTreeVM : BaseVM
    {
        public OrgTreeVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }

        public string Id { get; set; } = null!;
        public List<OrgTreeDTO> OrgTree { get; set; } = new();
      
    }
}
