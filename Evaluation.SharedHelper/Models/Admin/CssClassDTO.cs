

namespace Evaluation.SharedHelper.Models.Admin
{
    public class CssClassDTO : EntityBaseDTO
    {
        public string ClassName { get; set; } = null!;
        public string Styles { get; set; } = null!;
        public Guid ApplyTypeId { get; set; }
        public string? ApplyType { get; set; }

    }
}
