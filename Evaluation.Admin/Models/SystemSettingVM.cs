

using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models
{
    public class SystemSettingVM : BaseVM
    {
        public SystemSettingVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }

        public string Id { get; set; } = null!;
        public List<SystemSettingDTO> SystemSetting { get; set; } = new();

    }
}
