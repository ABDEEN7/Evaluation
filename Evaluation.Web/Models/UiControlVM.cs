


using Evaluation.SharedHelper.Models.Api;

namespace Evaluation.Web.Models
{
    public class UiControlVM 
    {

        public ControlValidationDTO Constraint { get; set; } = new();
        public UiControlItemDTO ControlItem { get; set; } = new();
        public string Lang { get; set; } = null!;
        
        public string ControlName { get; set; } = null!;
        public string? UibackendName { get; set; }

    }
}
