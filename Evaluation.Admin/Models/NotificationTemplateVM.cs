
using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models
{
    public class NotificationTemplateVM : BaseVM
    {
        public NotificationTemplateVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }

        public string Id { get; set; } = null!;
        public List<NotificationTemplateDTO> NotificationTemplate { get; set; } = new();
      
    }
}
