using Evaluation.Services.Shared;

namespace Evaluation.Admin.Models;

public class NdaStatusVM : BaseVM
{
    public NdaStatusVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {

    }
    public string? BackendName { get; set; }
    public string? NameAr { get; set; }
    public string? NameEn { get; set; }
}