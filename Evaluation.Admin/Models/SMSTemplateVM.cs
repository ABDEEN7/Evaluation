
using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models
{
    public class SMSTemplateVM : BaseVM
    {
        public SMSTemplateVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }

        public string Id { get; set; } = null!;
        public List<SMSTemplateDTO> SMSTemplate { get; set; } = new();

    }
}
