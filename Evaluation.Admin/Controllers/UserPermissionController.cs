using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.Models.Admin;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Evaluation.Admin.Controllers
{
    [Authorize]
    public class UserPermissionController : Controller
    {
      
        private readonly MasterBL masterBL;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly AzureBlobStorageService _blobService;
        public UserPermissionController( MasterBL masterBL, AzureBlobStorageService _blobService, IHttpContextAccessor httpContextAccessor)
        {

           
            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;
            this._blobService = _blobService;
        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_USER_PERMISSION })]
        public async Task<IActionResult> Index()
        {
            var model = new UserPermissionVM(httpContextAccessor);
            await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminUserPermission },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_USER_PERMISSION },
                new string[] {

                 ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT
            });
            return View(model);
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_USER_PERMISSION })]

        public async Task<IActionResult> GetAllUser([FromBody] AdminSearchDTO request)
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
            var response = await masterBL.GetAdminService<SrvUserPermissionBL>().GetAllUserList(request);
            return Ok(response);
        }

        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_USER_PERMISSION })]
        public async Task<IActionResult> GetRoleList()
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            var RoleList = await masterBL.GetAdminService<SrvUserPermissionBL>().GetRoleList();
            response.Add("RoleList", RoleList);
            return Ok(new ResponseEntity(response));
        }

        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_USER_PERMISSION })]
        public async Task<IActionResult> SaveUser()
        {
           
                var files = Request.Form.Files;
                var result = new UserProfileDTO();
                var model = Request.Form["request"][0]?.StringToObject<UserProfileDTO>();
                
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(model!, ConstantKeys.AdminPermission.ADD_ADMIN_USER_PERMISSION);
                if (validateObject)
                {
                    result = await masterBL.GetAdminService<SrvUserPermissionBL>().SaveUser(model!);
                }
                return Ok(new ResponseEntity(result));
           
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_USER_PERMISSION })]
        public async Task<IActionResult> UpdateUser()
        {
           
                var files = Request.Form.Files;
                var result = new UserProfileDTO();
                var model = Request.Form["request"][0]?.StringToObject<UserProfileDTO>();
               
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(model!, ConstantKeys.AdminPermission.ADD_ADMIN_USER_PERMISSION);
                if (validateObject)
                {
                    result = await masterBL.GetAdminService<SrvUserPermissionBL>().UpdateUser(model!);
                }
                return Ok(new ResponseEntity(result));
            
        }
        

    }
}
