namespace Evaluation.SharedHelper.Models.Admin;

public class GetScopeAcademicYearDTO: EntityBaseDTO
{
    public string? ScopeAcademicYearScopeParent { get; set; }
    public Guid? ScopeAcademicYearScopeParentId { get; set; }
    public string Department { get; set; }
    public string? Scope { get; set; }
    public string AcademicYear { get; set; }
    public Guid? ScopeParentId { get; set; }
    public Guid DepartmentId { get; set; }
    public Guid ScopeId { get; set; }
    public Guid AcademicYearId { get; set; }
}
