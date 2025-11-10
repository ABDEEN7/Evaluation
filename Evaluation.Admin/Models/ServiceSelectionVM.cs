

using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models
{
    public class ServiceSelectionVM : BaseVM
    {
        public ServiceSelectionVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }
        public string SystemModuleId { get; set; } = null!;
        public string ServiceId { get; set; } = null!;
        public List<SystemModuleDTO> SystemModules { get; set; } = new();
        public List<ServiceDTO> Services { get; set; } = new();
        public bool ShowServiceDefault { get; set; } = true;
    }
}
