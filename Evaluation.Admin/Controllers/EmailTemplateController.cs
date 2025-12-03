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
    public class EmailTemplateController : Controller
    {
        
        private readonly UserInfo userInfoSession;
        private readonly MasterBL masterBL;
        private readonly IHttpContextAccessor httpContextAccessor;

        public EmailTemplateController(UserInfo userInfoSession, MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
        {

            this.userInfoSession = userInfoSession;
            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;
           
        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_EMAILTEMPLATE })]
        public async Task<IActionResult> Index()
        {
            var model = new EmailTemplateVM(httpContextAccessor);
            await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminEmailTemplate },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_EMAILTEMPLATE },
                new string[] {
                 ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT
            });

            var property = typeof(EmailTemplate).GetProperty("OrderNo");
            model.containsOrderNo = property != null ? true : false;
            return View(model);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_EMAILTEMPLATE })]
        public async Task<IActionResult> GetAllEmailTemplate(int Page = 1)
        {
            var PageSize =  Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
            var response = await masterBL.GetAdminService<SrvEmailTemplateBL>().GetEmailTemplateList(Page, PageSize);
            return Ok(response);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_SERVICE })]
        public async Task<IActionResult> EmailTemplateDocument(Guid emailtemplateid)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            var EmailTemplateDocument = await masterBL.GetAdminService<SrvEmailTemplateBL>().GetEmailTemplateDocument(emailtemplateid);
            response.Add("EmailTemplateDocument", EmailTemplateDocument);
            return Ok(new ResponseEntity(response));
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_SERVICE })]
        public async Task<IActionResult> GetAllFileFields(Guid serviceid)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            var ServiceFields = await masterBL.GetAdminService<SrvEmailTemplateBL>().GetFieldFromService(serviceid);
            var EvaluationFields = await masterBL.GetAdminService<SrvEmailTemplateBL>().GetFieldFromEvaluation(serviceid);
            response.Add("ServiceFields", ServiceFields);
            response.Add("EvaluationFields", EvaluationFields);
            return Ok(new ResponseEntity(response));
        }

        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_EMAILTEMPLATE })]
        public async Task<IActionResult> SaveEmailTemplate()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<EmailTemplateDTO>();
            var EmailTemplateDocument = Request.Form["EmailTemplateDocument"][0]?.StringToObject<List<Guid>>();
            var result = new EmailTemplateDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_EMAILTEMPLATE);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvEmailTemplateBL>().SaveEmailTemplate(request!, EmailTemplateDocument);
                    
                }
                return Ok(result);
           
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_EMAILTEMPLATE })]
        public async Task<IActionResult> UpdateEmailTemplate()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<EmailTemplateDTO>();
            var EmailTemplateDocument = Request.Form["EmailTemplateDocument"][0]?.StringToObject<List<Guid>>();
            var result = new EmailTemplateDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_EMAILTEMPLATE);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvEmailTemplateBL>().UpdateEmailTemplate(request!, EmailTemplateDocument);
                }
                return Ok(result);
            
        }

        
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_EMAILTEMPLATE })]
        public async Task<IActionResult> DeleteEmailTemplate(Guid Id)
        {
           
                var result = await masterBL.GetAdminService<SrvEmailTemplateBL>().DeleteEmailTemplate(Id);
                return Ok(result);
           
        }

    }
}
