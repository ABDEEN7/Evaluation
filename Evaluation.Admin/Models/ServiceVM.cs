
using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;
namespace Evaluation.Admin.Models
{
    public class ServiceVM : BaseVM
    {
        public ServiceVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }

        public string Id { get; set; } = null!;
        public List<ServiceDTO> Service { get; set; } = new();

    }
}
