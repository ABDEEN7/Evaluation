
using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models
{
    public class TemplateDocumentVM : BaseVM
    {
        public TemplateDocumentVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }

        public string Id { get; set; } = null!;
        public List<TempLateDocDTO> SiteDocument { get; set; } = new();

    }
}
