


using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models
{
    public class SiteDocumentVM : BaseVM
    {
        public SiteDocumentVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }

        public string Id { get; set; } = null!;
        public List<SiteDocumentDTO> SiteDocument { get; set; } = new();

    }
}
