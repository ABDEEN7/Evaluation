using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models;

public class ScopeTypeVM : BaseVM
{
    public ScopeTypeVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {

    }

    public string Id { get; set; } = null!;
    public List<ScopeTypeDTO> Scopes { get; set; } = new();

}
