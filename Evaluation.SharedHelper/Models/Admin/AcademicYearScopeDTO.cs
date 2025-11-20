

namespace Evaluation.SharedHelper.Models.Admin
{
    public class AcademicYearScopeDTO : EntityBaseDTO
    {

        public Guid? ScopeId { get; set; }
        public Guid? ParentId { get; set; }
        public Guid? AcademicYearId { get; set; }
        public string? Scope { get; set; }
        public string? Parent { get; set; }
        public string? AcademicYear { get; set; }

    }
}
