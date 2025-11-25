

using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models
{
    public class UserPartyTypeVM : BaseVM
    {
        public UserPartyTypeVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }

        public string Id { get; set; } = null!;
        public List<UserPartyTypeDTO> UserPartyType { get; set; } = new();

    }
}
