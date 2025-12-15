using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Template;
using Evaluation.DAL.Models.Website;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.Models.Admin;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Evaluation.Admin.Controllers
{
    [Authorize]
    public class WebGroupsController : Controller
    {
        
        private readonly UserInfo userInfoSession;
        private readonly MasterBL masterBL;
        private readonly IHttpContextAccessor httpContextAccessor;

        public WebGroupsController(UserInfo userInfoSession, MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
        {

            this.userInfoSession = userInfoSession;
            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;
           
        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_WEBGROUPS })]
        public async Task<IActionResult> Index()
        {
            var model = new WebGroupsVM(httpContextAccessor);
            await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminWebGroups },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_WEBGROUPS },
                new string[] {
                 ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT
            });

            var property = typeof(WebGroup).GetProperty("OrderNo");
            model.containsOrderNo = property != null ? true : false;
            return View(model);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_WEBGROUPS })]
        public async Task<IActionResult> GetAllWebGroups(int Page = 1)
        {
            var PageSize =  Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
            var response = await masterBL.GetAdminService<SrvWebGroupsBL>().GetWebGroupsList(Page, PageSize);
            return Ok(response);
        }


        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_WEBGROUPS })]
        public async Task<IActionResult> SaveWebGroups()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<WebGroupsDTO>();
            var result = new WebGroupsDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_WEBGROUPS);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvWebGroupsBL>().SaveWebGroups(request!);
                    
                }
                return Ok(result);
           
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_WEBGROUPS })]
        public async Task<IActionResult> UpdateWebGroups()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<WebGroupsDTO>();
            var result = new WebGroupsDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_WEBGROUPS);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvWebGroupsBL>().UpdateWebGroups(request!);
                }
                return Ok(result);
            
        }

        
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_WEBGROUPS })]
        public async Task<IActionResult> DeleteWebGroups(Guid Id)
        {
           
                var result = await masterBL.GetAdminService<SrvWebGroupsBL>().DeleteWebGroups(Id);
                return Ok(result);
           
        }

    }
}
