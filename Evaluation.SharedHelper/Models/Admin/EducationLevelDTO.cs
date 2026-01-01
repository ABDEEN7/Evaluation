

namespace Evaluation.SharedHelper.Models.Admin
{
    public class EducationLevelDTO : EntityBaseDTO
    {
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string BackendName { get; set; } = null!;
        public int OrderNo { get; set; }

    }
}
