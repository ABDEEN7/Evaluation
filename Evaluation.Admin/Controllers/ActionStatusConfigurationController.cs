using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.Services.Models.Admin;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models.Admin;
using Evaluation.DAL.Helper;
using Evaluation.Services.BusinessLayer;
using Evaluation.DAL.Models.ActionEntities;

namespace Evaluation.Admin.Controllers
{
    [Authorize]
    public class ActionStatusConfigurationController : Controller
    {
        
        private readonly UserInfo userInfoSession;
        private readonly MasterBL masterBL;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ActionStatusConfigurationController(UserInfo userInfoSession, MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
        {

            this.userInfoSession = userInfoSession;
            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;
           
        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_ACTIONSTATUSCONFIGURATION })]
        public async Task<IActionResult> Index()
        {
            var model = new ActionStatusConfigurationVM(httpContextAccessor);
            await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminActionStatusConfiguration,
            ConstantKeys.AdminPages.AdminActionStatusConfigurationNotification},
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_ACTIONSTATUSCONFIGURATION,
                ConstantKeys.AdminPermission.ADD_ADMIN_ACTIONSTATUSCONFIGURATION_NOTIFICATION},
                new string[] {
                 ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT
            });

            var property = typeof(ActionStatusConfiguration).GetProperty("OrderNo");
            model.containsOrderNo = property != null ? true : false;
            return View(model);
        }
        
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_ACTIONSTATUSCONFIGURATION })]

        public async Task<IActionResult> GetAllActionStatusConfiguration([FromBody] AdminSearchDTO request)
        {

            if (null == request)
            {
                request = new AdminSearchDTO
                {
                    PageNum = 0,
                    PageSize = Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE)),
                };
            }
            else
            {
                request.PageNum = request.PageNum ?? 0;
                request.PageSize = request.PageSize ?? Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
            }
            request.PageNum = request.PageNum ?? 0;
            request.PageSize = request.PageSize ?? Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
            var response = await masterBL.GetAdminService<SrvActionStatusConfigurationBL>().GetActionStatusConfigurationList(request);
            return Ok(response);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_ACTIONSTATUSCONFIGURATION })]
        public async Task<IActionResult> GetActionStatusConfigurationByAction(Guid ServiceId, Guid actionId)
        {
            
            var response = await masterBL.GetAdminService<SrvActionStatusConfigurationBL>().GetActionStatusConfigurationByActionList(ServiceId,actionId);
            return Ok(response);
        }

        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_ACTIONSTATUSCONFIGURATION })]
        public async Task<IActionResult> GetActionStatusConfigurationByStatus(Guid ServiceId, Guid statusIdId)
        {

            var response = await masterBL.GetAdminService<SrvActionStatusConfigurationBL>().GetActionStatusConfigurationByStatusList(ServiceId,statusIdId);
            return Ok(response);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_ACTIONSTATUSCONFIGURATION })]
        public async Task<IActionResult> GetAllActionList(Guid ServiceId)
        {
            
            var response = await masterBL.GetAdminService<SrvActionStatusConfigurationBL>().GetAllActionList(ServiceId);
            return Ok(response);
        }

        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_ACTIONSTATUSCONFIGURATION })]
        public async Task<IActionResult> GetActionList(Guid ServiceId)
        {

            var response = await masterBL.GetAdminService<SrvActionStatusConfigurationBL>().GetActionList(ServiceId);
            return Ok(response);
        }

        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_ACTIONSTATUSCONFIGURATION })]
        public async Task<IActionResult> GetStatusList(Guid ServiceId)
        {

            var response = await masterBL.GetAdminService<SrvActionStatusConfigurationBL>().GetStatusList(ServiceId);
            return Ok(response);
        }

        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_ACTIONSTATUSCONFIGURATION })]
        public async Task<IActionResult> SaveActionStatusConfiguration()
        {
            
                var request = Request.Form["request"][0]?.StringToObject<ActionStatusConfigurationDTO>();
                var result = new ActionStatusConfigurationDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_ACTIONSTATUSCONFIGURATION);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvActionStatusConfigurationBL>().SaveActionStatusConfiguration(request!);
                    
                }
                return Ok(result);
            
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_ACTIONSTATUSCONFIGURATION })]
        public async Task<IActionResult> UpdateActionStatusConfiguration()
        {
            
                var request = Request.Form["request"][0]?.StringToObject<ActionStatusConfigurationDTO>();
                var result = new ActionStatusConfigurationDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_ACTIONSTATUSCONFIGURATION);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvActionStatusConfigurationBL>().UpdateActionStatusConfiguration(request!);
                }
            return Ok(result);

        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_ACTIONSTATUSCONFIGURATION })]
        public async Task<IActionResult> UpdateActionStatusConfigurationOrder()
        {

            var model = Request.Form["OrderObj"][0]?.StringToObject<List<OrderingDTO>>();
            var result = await masterBL.GetAdminService<SrvActionStatusConfigurationBL>().UpdateActionStatusConfigurationOrder(model!);
            return Ok(result);

        }

        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_ACTIONSTATUSCONFIGURATION })]
        public async Task<IActionResult> DeleteActionStatusConfiguration(Guid Id)
        {
            
                var result = await masterBL.GetAdminService<SrvActionStatusConfigurationBL>().DeleteActionStatusConfiguration(Id);
                return Ok(result);
            
        }

        
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_ACTIONSTATUSCONFIGURATION_NOTIFICATION })]
        public async Task<IActionResult> GetAllActionStatusConfigurationNotification(Guid actionstatusconfigid)
        {
            var response= await masterBL.GetAdminService<SrvActionStatusConfigurationBL>().GetActionStatusConfigurationNotificationList(actionstatusconfigid);
            return Ok(response);


        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_ACTIONSTATUSCONFIGURATION_NOTIFICATION })]
        public async Task<IActionResult> SaveActionStatusConfigurationNotification()
        {

            var request = Request.Form["request"][0]?.StringToObject<ActionStatusConfigNotificationDTO>();
            var result = new ActionStatusConfigNotificationDTO();
            bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_ACTIONSTATUSCONFIGURATION_NOTIFICATION);
            if (validateObject)
            {
                result = await masterBL.GetAdminService<SrvActionStatusConfigurationBL>().SaveActionStatusConfigurationNotification(request!);

            }
            return Ok(result);

        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_ACTIONSTATUSCONFIGURATION_NOTIFICATION })]
        public async Task<IActionResult> UpdateActionStatusConfigurationNotification()
        {

            var request = Request.Form["request"][0]?.StringToObject<ActionStatusConfigNotificationDTO>();
            var result = new ActionStatusConfigNotificationDTO();
            bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_ACTIONSTATUSCONFIGURATION_NOTIFICATION);
            if (validateObject)
            {
                result = await masterBL.GetAdminService<SrvActionStatusConfigurationBL>().UpdateActionStatusConfigurationNotification(request!);
            }
            return Ok(result);

        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_ACTIONSTATUSCONFIGURATION_NOTIFICATION })]
        public async Task<IActionResult> DeleteActionStatusConfigurationNotification(Guid Id)
        {

            var result = await masterBL.GetAdminService<SrvActionStatusConfigurationBL>().DeleteActionStatusConfigurationNotification(Id);
            return Ok(result);

        }
    }
}
