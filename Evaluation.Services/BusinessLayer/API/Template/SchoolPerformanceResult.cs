namespace Evaluation.Services.BusinessLayer.API.Template;

public class SchoolPerformanceResult
{
    public required string CriteriaName { get; set; }
    public int CriteriaWieght { get; set; }
    public required string CriteriaJudgement { get; set; }
    public decimal Average { get; set; }
}