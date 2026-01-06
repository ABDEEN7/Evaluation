using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models
{
    public class EvaluationPartiesVM : BaseVM
    {
        public EvaluationPartiesVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }

        public string Id { get; set; } = null!;
        public List<EvaluationPartiesDTO> EvaluationParties { get; set; } = new();
      
    }
}
