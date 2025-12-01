using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models
{
    public class ServiceActionVM : BaseVM
    {
        public ServiceActionVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }


        //Services
        public List<DropdownItem> PartyTypesList { get; set; }=new();
        public List<DropdownItem> TemplateDocsList { get; set; } = new();
       
        public List<DropdownItem> ActionTypes { get; set; } = new();
        public List<DropdownItem> Services { get; set; } = new();

        public List<ServiceActionDTO> ServiceActionList { get; set; } = new();
        public List<ControlValidationDTO> StatusPartyTypeDisplayNameList_Controls { get; set; } = new();
    }
}
