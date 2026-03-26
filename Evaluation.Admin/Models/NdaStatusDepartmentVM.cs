using Evaluation.Services.Shared;

namespace Evaluation.Admin.Models;

public class NdaStatusDepartmentVM : BaseVM
{
    public NdaStatusDepartmentVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {

    }
    public Guid? DepartmentId { get; set; }
    public Guid? NdaStatuisId { get; set; }
    public string? NameAr { get; set; }
    public string? NameEn { get; set; }
}
