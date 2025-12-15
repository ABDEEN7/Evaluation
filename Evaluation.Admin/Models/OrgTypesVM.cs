using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models
{
    public class OrgTypesVM : BaseVM
    {
        public OrgTypesVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }

        public string Id { get; set; } = null!;
        public List<OrgTypesDTO> OrgTypes { get; set; } = new();
      
    }
}
