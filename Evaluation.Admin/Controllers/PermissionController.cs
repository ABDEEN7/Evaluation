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
    public class PermissionController : Controller
    {
        
        private readonly UserInfo userInfoSession;
        private readonly MasterBL masterBL;
        private readonly IHttpContextAccessor httpContextAccessor;

        public PermissionController(UserInfo userInfoSession, MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
        {

            this.userInfoSession = userInfoSession;
            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;
           
        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_ROLE_PERMISSION })]
        public async Task<IActionResult> Index()
        {
            var model = new PermissionVM(httpContextAccessor);
            await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminPermission },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_ROLE_PERMISSION },
                new string[] {
                 ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT
            });

            var property = typeof(RolePermission).GetProperty("OrderNo");
            model.containsOrderNo = property != null ? true : false;
            return View(model);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_ROLE_PERMISSION })]
        public async Task<IActionResult> GetAllPagePermission()
        {
            
            var response = await masterBL.GetAdminService<SrvPermissionBL>().GetAllPagePermissionList();
            return Ok(response);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_ROLE_PERMISSION })]
        public async Task<IActionResult> GetRolePermission(Guid roleId)
        {

            var response = await masterBL.GetAdminService<SrvPermissionBL>().GetRolePermissionList(roleId);
            return Ok(response);
        }

        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_ROLE_PERMISSION })]
        public async Task<IActionResult> SaveRolePermission()
        {

            var request = Request.Form["request"][0]?.StringToObject<List<RolePermissionDTO>>();
            var result = new List<RolePermissionDTO>();
            bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_ROLE_PERMISSION);
            if (validateObject)
            {
                result = await masterBL.GetAdminService<SrvPermissionBL>().SaveRolePermission(request!);

            }
            return Ok(new ResponseEntity(result));

        }



    }
}
