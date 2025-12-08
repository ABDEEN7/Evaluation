using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.PermissionEntity;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.Models.Admin;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Admin.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        
        private readonly UserInfo userInfoSession;
        private readonly MasterBL masterBL;
        private readonly IHttpContextAccessor httpContextAccessor;

        public RoleController(UserInfo userInfoSession, MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
        {

            this.userInfoSession = userInfoSession;
            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;
           
        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_ROLE })]
        public async Task<IActionResult> Index()
        {
            var model = new RoleVM(httpContextAccessor);
            await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminRole },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_ROLE },
                new string[] {
                 ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT
            });

            var property = typeof(Role).GetProperty("OrderNo");
            model.containsOrderNo = property != null ? true : false;
            return View(model);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_ROLE })]
        public async Task<IActionResult> GetAllRole(int Page = 1)
        {
            var PageSize =  Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
            var response = await masterBL.GetAdminService<SrvRoleBL>().GetRoleList(Page, PageSize);
            return Ok(response);
        }



        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_ROLE })]
        public async Task<IActionResult> SaveRole()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<RoleDTO>();
            var cloneRoleValue = Request.Form["CloneRole"].FirstOrDefault();
            Guid? CloneRole = Guid.TryParse(cloneRoleValue, out var guidValue) ? guidValue : (Guid?)null;

            var result = new RoleDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_ROLE);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvRoleBL>().SaveRole(request!, CloneRole);
                    
                }
                return Ok(new ResponseEntity(result));
           
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_ROLE })]
        public async Task<IActionResult> UpdateRole()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<RoleDTO>();
            var result = new RoleDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_ROLE);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvRoleBL>().UpdateRole(request!);
                }
                return Ok(new ResponseEntity(result));
            
        }

       
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_ROLE })]
        public async Task<IActionResult> DeleteRole(Guid Id)
        {
           
                var result = await masterBL.GetAdminService<SrvRoleBL>().DeleteRole(Id);
                return Ok(result);
           
        }

    }
}
