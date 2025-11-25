

using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models
{
    public class PartyTypeVM : BaseVM
    {
        public PartyTypeVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }

        public string Id { get; set; } = null!;
        public List<PartyTypeDTO> PartyType { get; set; } = new();
      
    }
}
