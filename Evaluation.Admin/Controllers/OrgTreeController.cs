using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Models.Planing;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.Models.Admin;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Evaluation.Admin.Controllers
{
    [Authorize]
    public class OrgTreeController : Controller
    {
        
        private readonly UserInfo userInfoSession;
        private readonly MasterBL masterBL;
        private readonly IHttpContextAccessor httpContextAccessor;

        public OrgTreeController(UserInfo userInfoSession, MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
        {

            this.userInfoSession = userInfoSession;
            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;
           
        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_ORGTREE })]
        public async Task<IActionResult> Index()
        {
            var model = new OrgTreeVM(httpContextAccessor);
            await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminOrgTree },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_ORGTREE },
                new string[] {
                 ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT
            });

            var property = typeof(OrgTree).GetProperty("OrderNo");
            model.containsOrderNo = property != null ? true : false;
            return View(model);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_ORGTREE })]
        public async Task<IActionResult> GetAllOrgTree(int Page = 1)
        {
            var PageSize =  Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
            var response = await masterBL.GetAdminService<SrvOrgTreeBL>().GetOrgTreeList(Page, PageSize);
            return Ok(response);
        }


        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_ORGTREE })]
        public async Task<IActionResult> SaveOrgTree()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<OrgTreeDTO>();
            var result = new OrgTreeDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_ORGTREE);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvOrgTreeBL>().SaveOrgTree(request!);
                    
                }
                return Ok(result);
           
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_ORGTREE })]
        public async Task<IActionResult> UpdateOrgTree()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<OrgTreeDTO>();
            var result = new OrgTreeDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_ORGTREE);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvOrgTreeBL>().UpdateOrgTree(request!);
                }
                return Ok(result);
            
        }

        
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_ORGTREE })]
        public async Task<IActionResult> DeleteOrgTree(Guid Id)
        {
           
                var result = await masterBL.GetAdminService<SrvOrgTreeBL>().DeleteOrgTree(Id);
                return Ok(result);
           
        }

    }
}
