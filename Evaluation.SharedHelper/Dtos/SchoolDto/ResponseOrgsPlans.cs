namespace Evaluation.SharedHelper.Dtos.SchoolDto;

public class ResponseOrgsPlans
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime? LastEvaluationDate { get; set; }
    public string? Rating { get; set; }
    //public int StudentNumber { get; set; }
    public int AcademicYear { get; set; }
    public List<SchoolLevelDto>? SchoolLevel { get; set; }
    public ParentOrgTreeDto? OrgParent { get; set; }
}