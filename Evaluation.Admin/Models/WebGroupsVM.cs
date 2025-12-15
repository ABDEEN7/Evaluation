using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models
{
    public class WebGroupsVM : BaseVM
    {
        public WebGroupsVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }

        public string Id { get; set; } = null!;
        public List<WebGroupsDTO> WebGroups { get; set; } = new();
      
    }
}
