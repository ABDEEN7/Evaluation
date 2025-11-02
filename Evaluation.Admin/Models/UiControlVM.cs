using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models
{
    public class UiControlVM : BaseVM
    {
        public UiControlVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }

        public ControlValidationDTO Constraint { get; set; } = new();
        public UiControlItemDTO ControlItem { get; set; } = new();
        public string Lang { get; set; } = null!;
        
        public string ControlName { get; set; } = null!;
        public string? UibackendName { get; set; }

    }
}
