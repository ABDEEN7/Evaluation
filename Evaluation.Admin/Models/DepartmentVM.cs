
using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;
namespace Evaluation.Admin.Models
{
    public class DepartmentVM : BaseVM
    {
        public DepartmentVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }

        public string Id { get; set; } = null!;
        public List<DepartmentDTO> Service { get; set; } = new();

    }
}
