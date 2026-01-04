

using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models
{
    public class EmployeesVM : BaseVM
    {
        public EmployeesVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }

        public string Id { get; set; } = null!;
        public List<EmployeesDTO> Employees { get; set; } = new();
      
    }
}
