
using Evaluation.Services.Shared;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Admin.Models
{
    public class ServiceStatusVM : BaseVM
    {
        public ServiceStatusVM(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }


        //Services
        //public List<DropdownItem> StatusGroups { get; set; }
        public List<DropdownItem> PartyTypesList { get; set; } = new();
        public List<DropdownItem> Services { get; set; } = new();
        public List<ServiceStatusDTO> ServiceStatusList { get; set; } = new();
        public List<DropdownItem> ActionTypes { get; set; } = new();
        public List<ControlValidationDTO> StatusPartyTypeDisplayNameList_Controls { get; set; } = new();

        public List<DropdownItem> ServiceStatusTypeList { get; set; } = new();
    }
}
