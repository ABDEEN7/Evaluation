using Evaluation.Services.Shared;

namespace Evaluation.Admin.Models;

public class DepEvaluationTypeVM : BaseVM
{
    public DepEvaluationTypeVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
    }

    public string? BackendName { get; set; }
    public string? NameAr { get; set; }
    public string? NameEn { get; set; }
}
