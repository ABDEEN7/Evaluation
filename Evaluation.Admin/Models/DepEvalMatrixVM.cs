using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models;

public class DepEvalMatrixVM : BaseVM
{
    public DepEvalMatrixVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {

    }
    public string Id { get; set; } = null!;
    public List<DepEvalMatrixDTO> FormEvalMatrix { get; set; } = new();

}