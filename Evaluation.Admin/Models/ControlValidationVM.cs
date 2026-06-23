using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models;

public class ControlValidationVM : BaseVM
{
    public ControlValidationVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {

    }

    public string Id { get; set; } = null!;
    public List<ControlValidationDTO> ControlValidation { get; set; } = new();

}