using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models;

public class FormEvalMatrixVM : BaseVM
{
    public FormEvalMatrixVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {

    }
    public string Id { get; set; } = null!;
    public List<FormEvalMatrixDTO> FormEvalMatrix { get; set; } = new();

}
