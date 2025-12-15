using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.Models.Admin;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Evaluation.Admin.Controllers
{
    [Authorize]
    public class ScopesController : Controller
    {
        
        private readonly UserInfo userInfoSession;
        private readonly MasterBL masterBL;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ScopesController(UserInfo userInfoSession, MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
        {

            this.userInfoSession = userInfoSession;
            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;
           
        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_SCOPES })]
        public async Task<IActionResult> Index()
        {
            var model = new ScopesVM(httpContextAccessor);
            await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminScopes },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_SCOPES },
                new string[] {
                 ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT
            });

            var property = typeof(Scope).GetProperty("OrderNo");
            model.containsOrderNo = property != null ? true : false;
            return View(model);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_SCOPES })]
        public async Task<IActionResult> GetAllScopes(int Page = 1)
        {
            var PageSize =  Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
            var response = await masterBL.GetAdminService<SrvScopesBL>().GetScopesList(Page, PageSize);
            return Ok(response);
        }


        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_SCOPES })]
        public async Task<IActionResult> SaveScopes()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<ScopesDTO>();
            var result = new ScopesDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_SCOPES);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvScopesBL>().SaveScopes(request!);
                    
                }
                return Ok(result);
           
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_SCOPES })]
        public async Task<IActionResult> UpdateScopes()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<ScopesDTO>();
            var result = new ScopesDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_SCOPES);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvScopesBL>().UpdateScopes(request!);
                }
                return Ok(result);
            
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_SCOPES })]
        public async Task<IActionResult> UpdateScopesOrder()
        {

            var model = Request.Form["OrderObj"][0]?.StringToObject<List<OrderingDTO>>();
            var result = await masterBL.GetAdminService<SrvScopesBL>().UpdateScopesOrder(model!);
            return Ok(result);

        }

        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_SCOPES })]
        public async Task<IActionResult> DeleteScopes(Guid Id)
        {
           
                var result = await masterBL.GetAdminService<SrvScopesBL>().DeleteScopes(Id);
                return Ok(result);
           
        }

    }
}
