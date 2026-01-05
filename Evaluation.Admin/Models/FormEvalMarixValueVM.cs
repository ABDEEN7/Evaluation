using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models
{
    public class FormEvalMarixValueVM : BaseVM
    {
        public FormEvalMarixValueVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }

        public string Id { get; set; } = null!;
        public List<FormEvalMarixValueDTO> FormEvalMarixValue { get; set; } = new();
      
    }
}
