

namespace Evaluation.SharedHelper.Models.Admin
{
    public class UiControlItemDTO
    {
        public ControlValidationDTO Constraint { get; set; } = new();
        public UiControlDTO Control { get; set; } = new();
        public string Lang { get; set; } = null!;
        public string ControlName { get; set; } = null!;
        public string ControlType { get; set; } = null!;
        public string? UibackendName { get; set; }
        public string? TabulatorConfig { get; set; }
        public string? ControlJsonConfig { get; set; }
        public int? RowOrder { get; set; }
    }
}
