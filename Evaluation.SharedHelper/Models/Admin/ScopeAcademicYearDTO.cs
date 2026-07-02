

namespace Evaluation.SharedHelper.Models.Admin
{
    public class ScopeAcademicYearDTO : EntityBaseDTO
    {

        public Guid? scopeAcademicYearScopeParent { get; set; }
        public Guid? ScopeAcademicYearScopeParentId { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid ScopeId { get; set; }
        public Guid? Scope { get; set; }
        public Guid AcademicYearId { get; set; }
        public string AcademicYear { get; set; } = null!;

    }
}
