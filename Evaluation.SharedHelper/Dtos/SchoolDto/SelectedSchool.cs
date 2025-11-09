namespace Evaluation.SharedHelper.Dtos.SchoolDto;

public class SelectedSchool
{
    public Guid Id { get; set; }
    public DateTime? StartEvaluationDate { get; set; }
    public DateTime? EndEvaluationDate { get; set; }
    public Guid VisitTypeId { get; set; }
}