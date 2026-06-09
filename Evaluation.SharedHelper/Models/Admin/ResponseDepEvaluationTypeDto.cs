using Evaluation.DAL.Models.DepartementEntites;

namespace Evaluation.SharedHelper.Models.Admin;

public class ResponseDepEvaluationTypeDto : EntityBaseDTO
{
    public string Department { get; set; }
    public string EvaluationType { get; set; }
    public string BackendName { get; set; } = null!;
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public int OrderNo { get; set; }
}
