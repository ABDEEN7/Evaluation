using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models
{
    public class FormGroupVM : BaseVM
    {
        public FormGroupVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }

        public string Id { get; set; } = null!;

        public List<FormGroupDTO> FormGroup { get; set; } = new();

    }
}
