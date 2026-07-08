

namespace Evaluation.SharedHelper.Models.Admin
{
    public class ScopeAcademicYearDTO : EntityBaseDTO
    {

        public string? ScopeAcademicYearScopeParent { get; set; }
        public Guid? ScopeAcademicYearScopeParentId { get; set; }
        public Guid DepartmentId { get; set; }
        public string? Department { get; set; }
        public Guid ScopeId { get; set; }
        public string? Scope { get; set; }
        public Guid AcademicYearId { get; set; }
        public string AcademicYear { get; set; } = null!;

    }
}
