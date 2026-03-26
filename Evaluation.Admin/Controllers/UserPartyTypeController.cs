using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.Models.Admin;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Admin.Controllers
{
    [Authorize]
    public class UserPartyTypeController : Controller
    {
        
        private readonly UserInfo userInfoSession;
        private readonly MasterBL masterBL;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserPartyTypeController(UserInfo userInfoSession, MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
        {

            this.userInfoSession = userInfoSession;
            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;
           
        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_USERPARTYTYPE })]
        public async Task<IActionResult> Index()
        {
            var model = new UserPartyTypeVM(httpContextAccessor);
            await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminUserPartyType, ConstantKeys.AdminPages.AdminUserPartyTypeSignature },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_USERPARTYTYPE,
                ConstantKeys.AdminPermission.ADD_ADMIN_USERPARTYTYPE_SIGNATURE},
                new string[] {
                 ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT,
                 ConstantKeys.AdminSettings.UserPartyTypeSignatureHeight,
                 ConstantKeys.AdminSettings.UserPartyTypeSignatureWidth
            });
           
            var property = typeof(UserPartyType).GetProperty("OrderNo");
            model.containsOrderNo = property != null ? true : false;
            return View(model);
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_USERPARTYTYPE })]
        public async Task<IActionResult> GetAllUserPartyType([FromBody] AdminSearchDTO request)
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
            var response = await masterBL.GetAdminService<SrvUserPartyTypeBL>().GetUserPartyTypeList(request);
            return Ok(response);
        }

        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_USERPARTYTYPE_SIGNATURE })]
        public async Task<IActionResult> GetAllUserPartyTypeSignature(Guid userpartytypeid)
        {
            var PageSize = masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE).FirstOrDefault();
            var response = await masterBL.GetAdminService<SrvUserPartyTypeBL>().GetUserPartyTypeSignatureList(userpartytypeid);
            return Ok(response);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_USERPARTYTYPE })]
        public async Task<IActionResult> GetDepartment()
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            var Department = await masterBL.GetAdminService<SrvUserPartyTypeBL>().GetDepartment();
            response.Add("Department", Department);
            return Ok(new ResponseEntity(response));
        }

        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_USERPARTYTYPE })]
        public async Task<IActionResult> SaveUserPartyType()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<UserPartyTypeDTO>();
           
            var result = new UserPartyTypeDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_USERPARTYTYPE);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvUserPartyTypeBL>().SaveUserPartyType(request!);
                    
                }
                return Ok(new ResponseEntity(result));
           
        }

        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_USERPARTYTYPE_SIGNATURE })]
        public async Task<IActionResult> SaveUserPartyTypeSignature()
        {

            var request = Request.Form["request"][0]?.StringToObject<UserPartyTypeSignatureDTO>();
            var files = Request.Form.Files;
            var result = new UserPartyTypeSignatureDTO();
            long maxheight =long.Parse(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.UserPartyTypeSignatureHeight));
            long maxwidth = long.Parse(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.UserPartyTypeSignatureWidth));
            if (null != files)
            {
                var FinalFiles = files.Where(c => c.Length > 0).ToList();

                foreach (var item in FinalFiles)
                {

                    System.Drawing.Image img = System.Drawing.Image.FromStream(item.OpenReadStream());
                    if (img.Width > maxwidth)
                    {
                        throw new BusinessException(ConstantKeys.ExceptionMessage.UserPartyTypeSignatureWidthError);
                    }
                    if (img.Height > maxheight)
                    {
                        throw new BusinessException(ConstantKeys.ExceptionMessage.UserPartyTypeSignatureHeightError);
                    }

                    using (var ms = new MemoryStream())
                    {
                        item.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        request.Signature = fileBytes;
                        // act on the Base64 data
                    }

                }
            }

            bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_USERPARTYTYPE_SIGNATURE);
            if (validateObject)
            {
                result = await masterBL.GetAdminService<SrvUserPartyTypeBL>().SaveUserPartyTypeSignature(request!);

            }
            return Ok(new ResponseEntity(result));

        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_USERPARTYTYPE })]
        public async Task<IActionResult> UpdateUserPartyType()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<UserPartyTypeDTO>();
            
            var result = new UserPartyTypeDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_USERPARTYTYPE);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvUserPartyTypeBL>().UpdateUserPartyType(request!);
                }
                return Ok(new ResponseEntity(result));
            
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_USERPARTYTYPE_SIGNATURE })]
        public async Task<IActionResult> UpdateUserPartyTypeSignature()
        {

            var request = Request.Form["request"][0]?.StringToObject<UserPartyTypeSignatureDTO>();
            var files = Request.Form.Files;
            var result = new UserPartyTypeSignatureDTO();

            bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_USERPARTYTYPE_SIGNATURE);
            if (validateObject)
            {
                result = await masterBL.GetAdminService<SrvUserPartyTypeBL>().UpdateUserPartyTypeSignature(request!);

            }
            return Ok(new ResponseEntity(result));

        }

        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_USERPARTYTYPE })]
        public async Task<IActionResult> DeleteUserPartyType(Guid Id)
        {
           
                var result = await masterBL.GetAdminService<SrvUserPartyTypeBL>().DeleteUserPartyType(Id);
                return Ok(result);
           
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_USERPARTYTYPE_SIGNATURE })]
        public async Task<IActionResult> DeleteUserPartyTypeSignature(Guid Id)
        {

            var result = await masterBL.GetAdminService<SrvUserPartyTypeBL>().DeleteUserPartyTypeSignature(Id);
            return Ok(result);

        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_USERPARTYTYPE })]
        public async Task<IActionResult> GetAllUserList()
        {

            Dictionary<string, object> response = new Dictionary<string, object>();
            var User = await masterBL.GetAdminService<SrvUserPartyTypeBL>().GetAllUserList();
            response.Add("User", User);
            return Ok(new ResponseEntity(response));
        }

    }
}
