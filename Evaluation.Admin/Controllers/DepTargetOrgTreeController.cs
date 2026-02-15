using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.Org;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.Models.Admin;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Evaluation.Admin.Controllers
{
    [Authorize]
    public class DepTargetOrgTreeController : Controller
    {
        
        private readonly UserInfo userInfoSession;
        private readonly MasterBL masterBL;
        private readonly IHttpContextAccessor httpContextAccessor;

        public DepTargetOrgTreeController(UserInfo userInfoSession, MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
        {

            this.userInfoSession = userInfoSession;
            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;
           
        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_DEPTARGETORGTREE })]
        public async Task<IActionResult> Index()
        {
            var model = new DepTargetOrgTreeVM(httpContextAccessor);
            await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminDepTargetOrgTree },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_DEPTARGETORGTREE },
                new string[] {
                 ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT
            });

            var property = typeof(DepTargetOrgTree).GetProperty("OrderNo");
            model.containsOrderNo = property != null ? true : false;
            return View(model);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_DEPTARGETORGTREE })]
        public async Task<IActionResult> GetAllDepTargetOrgTree(int Page = 1)
        {
            var PageSize =  Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
            var response = await masterBL.GetAdminService<SrvDepTargetOrgTreeBL>().GetDepTargetOrgTreeList(Page, PageSize);
            return Ok(response);
        }


        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_DEPTARGETORGTREE })]
        public async Task<IActionResult> SaveDepTargetOrgTree()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<DepTargetOrgTreeDTO>();
            var result = new DepTargetOrgTreeDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_DEPTARGETORGTREE);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvDepTargetOrgTreeBL>().SaveDepTargetOrgTree(request!);
                    
                }
                return Ok(result);
           
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_DEPTARGETORGTREE })]
        public async Task<IActionResult> UpdateDepTargetOrgTree()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<DepTargetOrgTreeDTO>();
            var result = new DepTargetOrgTreeDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_DEPTARGETORGTREE);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvDepTargetOrgTreeBL>().UpdateDepTargetOrgTree(request!);
                }
                return Ok(result);
            
        }
        

        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_DEPTARGETORGTREE })]
        public async Task<IActionResult> DeleteDepTargetOrgTree(Guid Id)
        {
           
                var result = await masterBL.GetAdminService<SrvDepTargetOrgTreeBL>().DeleteDepTargetOrgTree(Id);
                return Ok(result);
           
        }

    }
}
