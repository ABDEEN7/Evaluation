using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models
{
    public class SchoolTypesVM : BaseVM
    {
        public SchoolTypesVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }

        public string Id { get; set; } = null!;
        public List<SchoolTypesDTO> SchoolTypes { get; set; } = new();
      
    }
}
