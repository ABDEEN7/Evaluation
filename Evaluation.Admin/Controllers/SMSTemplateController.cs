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
    public class SMSTemplateController : Controller
    {
        
        private readonly UserInfo userInfoSession;
        private readonly MasterBL masterBL;
        private readonly IHttpContextAccessor httpContextAccessor;

        public SMSTemplateController(UserInfo userInfoSession, MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
        {

            this.userInfoSession = userInfoSession;
            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;
           
        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_SMSTEMPLATE })]
        public async Task<IActionResult> Index()
        {
            var model = new SMSTemplateVM(httpContextAccessor);
            await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminSMSTemplate },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_SMSTEMPLATE },
                new string[] {
                 ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT
            });

            var property = typeof(SMSTemplate).GetProperty("OrderNo");
            model.containsOrderNo = property != null ? true : false;
            return View(model);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_SMSTEMPLATE })]
        public async Task<IActionResult> GetAllSMSTemplate(int Page = 1)
        {
            var PageSize =  Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
            var response = await masterBL.GetAdminService<SrvSMSTemplateBL>().GetSMSTemplateList(Page, PageSize);
            return Ok(response);
        }
        

        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_SMSTEMPLATE })]
        public async Task<IActionResult> SaveSMSTemplate()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<SMSTemplateDTO>();
            var result = new SMSTemplateDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_SMSTEMPLATE);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvSMSTemplateBL>().SaveSMSTemplate(request!);
                    
                }
                return Ok(new ResponseEntity(result));
           
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_SMSTEMPLATE })]
        public async Task<IActionResult> UpdateSMSTemplate()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<SMSTemplateDTO>();
            var result = new SMSTemplateDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_SMSTEMPLATE);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvSMSTemplateBL>().UpdateSMSTemplate(request!);
                }
                return Ok(new ResponseEntity(result));
            
        }

        
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_SMSTEMPLATE })]
        public async Task<IActionResult> DeleteSMSTemplate(Guid Id)
        {
           
                var result = await masterBL.GetAdminService<SrvSMSTemplateBL>().DeleteSMSTemplate(Id);
                return Ok(result);
           
        }

    }
}
