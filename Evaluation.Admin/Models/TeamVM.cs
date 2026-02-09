using Evaluation.Services.Shared;

namespace Evaluation.Admin.Models;

public class TeamVM : BaseVM
{
    public TeamVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        
    }
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public Guid DepartmentId { get; set; }
}
