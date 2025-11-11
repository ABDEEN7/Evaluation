
using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models
{
    public class ActionStatusConfigurationVM : BaseVM
    {
        public ActionStatusConfigurationVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }

        public string Id { get; set; } = null!;
        public List<ActionStatusConfigurationDTO> ActionStatusConfiguration { get; set; } = new();
       
      
    }
}
