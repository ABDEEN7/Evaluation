using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.ServiceRequestEntities;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.Models.Admin;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace Evaluation.Admin.Controllers
{
    [Authorize]
    public class ServiceStatusConfigurationController : Controller
    {
        
        private readonly UserInfo userInfoSession;
        private readonly MasterBL masterBL;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ServiceStatusConfigurationController(UserInfo userInfoSession, MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
        {

            this.userInfoSession = userInfoSession;
            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;
           
        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_ServiceStatusConfiguration })]
        public async Task<IActionResult> Index()
        {
            var model = new ServiceStatusConfigurationVM(httpContextAccessor);
            await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminServiceStatusConfiguration },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_ServiceStatusConfiguration},
                new string[] {
                 ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT
            });
          
            var property = typeof(ServiceStatusConfiguration).GetProperty("OrderNo");
            model.containsOrderNo = property != null ? true : false;
            return View(model);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_ServiceStatusConfiguration })]
        public async Task<IActionResult> GetAllServiceStatusConfiguration(Guid ServiceId, Guid systemModuleId, int Page = 1)
        {
            var PageSize =  Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
            var response = await masterBL.GetAdminService<SrvServiceStatusConfigurationBL>().GetServiceStatusConfigurationList(Page, PageSize,ServiceId, systemModuleId);
            return Ok(response);
        }


        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_ServiceStatusConfiguration })]
        public async Task<IActionResult> SaveServiceStatusConfiguration()
        {
            
                var request = Request.Form["request"][0]?.StringToObject<ServiceStatusConfigurationDTO>();
                var result = new ServiceStatusConfigurationDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_ServiceStatusConfiguration);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvServiceStatusConfigurationBL>().SaveServiceStatusConfiguration(request!);
                    
                }
                return Ok(new ResponseEntity(result));
            
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_ServiceStatusConfiguration })]
        public async Task<IActionResult> UpdateServiceStatusConfiguration()
        {
            
                var request = Request.Form["request"][0]?.StringToObject<ServiceStatusConfigurationDTO>();
                var result = new ServiceStatusConfigurationDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_ServiceStatusConfiguration);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvServiceStatusConfigurationBL>().UpdateServiceStatusConfiguration(request!);
                }
                return Ok(new ResponseEntity(result));
           
        }
        

        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_ServiceStatusConfiguration })]
        public async Task<IActionResult> DeleteServiceStatusConfiguration(Guid Id)
        {
            
                var result = await masterBL.GetAdminService<SrvServiceStatusConfigurationBL>().DeleteServiceStatusConfiguration(Id);
                return Ok(result);
            
        }
        
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_ServiceStatusConfiguration })]
        public async Task<IActionResult> GetAllServiceStatus(Guid systemModuleId, Guid departmentId)
        {

            Dictionary<string, object> response = new Dictionary<string, object>();
            var User = await masterBL.GetAdminService<SrvServiceStatusConfigurationBL>().GetServiceStatuis(systemModuleId, departmentId);
            response.Add("ServiceStatuis", User);
            return Ok(new ResponseEntity(response));
        }



    }
}
