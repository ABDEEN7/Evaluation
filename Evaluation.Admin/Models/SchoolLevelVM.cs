using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models
{
    public class SchoolLevelVM : BaseVM
    {
        public SchoolLevelVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }

        public string Id { get; set; } = null!;
        public List<SchoolLevelDTO> SchoolLevel { get; set; } = new();
      
    }
}
