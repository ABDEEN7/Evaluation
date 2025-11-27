

using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models
{
    public class ScopeAcademicYearVM : BaseVM
    {
        public ScopeAcademicYearVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }
        public string Id { get; set; } = null!;
        public List<ScopeAcademicYearDTO> ScopeAcademicYear { get; set; } = new();

    }
}
