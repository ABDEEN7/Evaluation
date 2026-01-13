

namespace Evaluation.SharedHelper.Models.Admin
{
    public class SchoolLevelDTO : EntityBaseDTO
    {
        public string? NSISCode { get; set; }
        public int Year { get; set; }
        public Guid SchoolId { get; set; }
        public string? School { get; set; } = null!;
        public Guid EducationLevelId { get; set; }
        public string? EducationLevel { get; set; } = null!;

    }
}
