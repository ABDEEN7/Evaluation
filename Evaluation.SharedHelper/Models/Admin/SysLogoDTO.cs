

namespace Evaluation.SharedHelper.Models.Admin
{
	public class SysLogoDTO : EntityBaseDTO
    {
        public string WebLogoAr { get; set; } = null!;
        public string WebLogoEn { get; set; } = null!;
        public string AdminLogoAr { get; set; } = null!;
        public string AdminLogoEn { get; set; } = null!;
        public string Favicon { get; set; } = null!;
    }
}
