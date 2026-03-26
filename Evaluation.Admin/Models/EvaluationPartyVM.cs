using Evaluation.Services.Shared;

namespace Evaluation.Admin.Models;

public class EvaluationPartyVM : BaseVM
{
    public EvaluationPartyVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {

    }
    public string? NameAr { get; set; }
    public string? NameEn { get; set; }
    public Guid DepartmentId { get; set; }
}
