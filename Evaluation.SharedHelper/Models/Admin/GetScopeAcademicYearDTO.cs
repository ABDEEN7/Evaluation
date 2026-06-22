namespace Evaluation.SharedHelper.Models.Admin;

public class GetScopeAcademicYearDTO: EntityBaseDTO
{
    public string? ScopeAcademicYearScopeParent { get; set; }
    public string Department { get; set; }
    public string? Scope { get; set; }
    public string AcademicYear { get; set; }    
}
