namespace Evaluation.SharedHelper.Dtos.SchoolDto;

public class ResponseOrgsPlans
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public DateOnly? LastEvaluationDate { get; set; }
    public string? Rating { get; set; }
    //public int StudentNumber { get; set; }
    public DateOnly? AcademicYear { get; set; }
    public int? YearAcdemicYear { get; set; }
    public List<SchoolLevelDto>? SchoolLevel { get; set; }
    public ParentOrgTreeDto? OrgParent { get; set; }
}