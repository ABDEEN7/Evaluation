using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models
{
    public class EvaluationTypeVM : BaseVM
    {
        public EvaluationTypeVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }

        public string Id { get; set; } = null!;
        public List<EvaluationTypeDTO> EvaluationType { get; set; } = new();
      
    }
}
