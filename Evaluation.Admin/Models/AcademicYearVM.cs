

using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models
{
    public class AcademicYearVM : BaseVM
    {
        public AcademicYearVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }
        public string Id { get; set; } = null!;
        public List<AcademicYearDTO> AcademicYear { get; set; } = new();

    }
}
