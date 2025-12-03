using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Template;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.Models.Admin;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Evaluation.Admin.Controllers
{
    [Authorize]
    public class NotificationTemplateController : Controller
    {
        
        private readonly UserInfo userInfoSession;
        private readonly MasterBL masterBL;
        private readonly IHttpContextAccessor httpContextAccessor;

        public NotificationTemplateController(UserInfo userInfoSession, MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
        {

            this.userInfoSession = userInfoSession;
            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;
           
        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_NOTIFICATION_TEMPLATE })]
        public async Task<IActionResult> Index()
        {
            var model = new NotificationTemplateVM(httpContextAccessor);
            await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminNotificationTemplate },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_NOTIFICATION_TEMPLATE },
                new string[] {
                 ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT
            });

            var property = typeof(NotificationTemplate).GetProperty("OrderNo");
            model.containsOrderNo = property != null ? true : false;
            return View(model);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_NOTIFICATION_TEMPLATE })]
        public async Task<IActionResult> GetAllNotificationTemplate(Guid systemmoduleid, int Page = 1)
        {
            var PageSize =  Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
            var response = await masterBL.GetAdminService<SrvNotificationTemplateBL>().GetNotificationTemplateList(systemmoduleid,Page, PageSize);
            return Ok(response);
        }
       
        
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_NOTIFICATION_TEMPLATE })]
        public async Task<IActionResult> SaveNotificationTemplate()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<NotificationTemplateDTO>();
                var result = new NotificationTemplateDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_NOTIFICATION_TEMPLATE);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvNotificationTemplateBL>().SaveNotificationTemplate(request!);
                    
                }
                return Ok(new ResponseEntity(result));
           
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_NOTIFICATION_TEMPLATE })]
        public async Task<IActionResult> UpdateNotificationTemplate()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<NotificationTemplateDTO>();
                var result = new NotificationTemplateDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_NOTIFICATION_TEMPLATE);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvNotificationTemplateBL>().UpdateNotificationTemplate(request!);
                }
                return Ok(new ResponseEntity(result));
            
        }

        
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_NOTIFICATION_TEMPLATE })]
        public async Task<IActionResult> DeleteNotificationTemplate(Guid Id)
        {
           
                var result = await masterBL.GetAdminService<SrvNotificationTemplateBL>().DeleteNotificationTemplate(Id);
                return Ok(result);
           
        }

    }
}
