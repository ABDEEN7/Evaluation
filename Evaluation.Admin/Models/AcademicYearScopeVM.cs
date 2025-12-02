

using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models
{
    public class AcademicYearScopeVM : BaseVM
    {
        public AcademicYearScopeVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }
        public string Id { get; set; } = null!;
        public List<AcademicYearScopeDTO> AcademicYearScope { get; set; } = new();

    }
}
