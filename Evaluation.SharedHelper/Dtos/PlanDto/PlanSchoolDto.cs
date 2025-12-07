namespace Evaluation.DAL.Dtos;

public class PlanSchoolDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime? StartEvaluationDate { get; set; }
    public DateTime? EndEvaluationDate { get; set; }
    public Guid? VisitTypeId { get; set; }
    public string VisitTypeName { get; set; }
}