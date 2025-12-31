using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models
{
    public class EducationLevelVM : BaseVM
    {
        public EducationLevelVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }

        public string Id { get; set; } = null!;
        public List<EducationLevelDTO> EducationLevel { get; set; } = new();
      
    }
}
