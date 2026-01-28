using Evaluation.SharedHelper.Enums;

namespace Evaluation.SharedHelper.Dtos.SchoolDto;

public class SchoolRequest : PaginatedQuery
{
    public string? Name { get; set; }
    public DateOnly? EstablishmentDate { get; set; }
    public DateTime? VisitDateFrom { get; set; }
    public DateTime? VisitDateTo { get; set; }
    public DateTime? LastEvaluationDateFrom { get; set; }
    public DateTime? LastEvaluationDateTo { get; set; }
    public Guid? VisitType { get; set; }
    public int? AcademicYear { get; set; }
    public string? DepartmentRoutingPath { get; set; }
    public Guid StatusId { get; set; }
    public Guid? ParentId { get; set; }

}
