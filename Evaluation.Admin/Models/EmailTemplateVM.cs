using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models
{
    public class EmailTemplateVM : BaseVM
    {
        public EmailTemplateVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }

        public string Id { get; set; } = null!;
        public List<EmailTemplateDTO> EmailTemplate { get; set; } = new();
      
    }
}
