using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.FormBuilder;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.Models.Admin;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace Evaluation.Admin.Controllers
{
    [Authorize]
    public class DropDownTypeController : Controller
    {
        
        private readonly UserInfo userInfoSession;
        private readonly MasterBL masterBL;
        private readonly IHttpContextAccessor httpContextAccessor;

        public DropDownTypeController(UserInfo userInfoSession, MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
        {

            this.userInfoSession = userInfoSession;
            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;
           
        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_NAVBAR })]
        public async Task<IActionResult> Index()
        {
            var model = new DropDownTypeVM(httpContextAccessor);
            await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminDropDownType },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_DROPDOWNTYPE },
                new string[] {
                 ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT
            });

            var property = typeof(DropDownType).GetProperty("OrderNo");
            model.containsOrderNo = property != null ? true : false;
            return View(model);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_DROPDOWNTYPE })]
        public async Task<IActionResult> GetAllDropDownType(Guid id,int Page = 1)
        {
            var PageSize =  Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
            var response = await masterBL.GetAdminService<SrvDropDownTypeBL>().GetDropDownTypeList(id,Page, PageSize);
            return Ok(response);
        }
       
        
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_DROPDOWNTYPE })]
        public async Task<IActionResult> SaveDropDownType()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<DropDownTypeDTO>();
                var result = new DropDownTypeDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_DROPDOWNTYPE);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvDropDownTypeBL>().SaveDropDownType(request!);
                    
                }
                return Ok(new ResponseEntity(result));
           
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_DROPDOWNTYPE })]
        public async Task<IActionResult> UpdateDropDownType()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<DropDownTypeDTO>();
                var result = new DropDownTypeDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_DROPDOWNTYPE);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvDropDownTypeBL>().UpdateDropDownType(request!);
                }
                return Ok(new ResponseEntity(result));
            
        }

        
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_DROPDOWNTYPE })]
        public async Task<IActionResult> DeleteDropDownType(Guid Id)
        {
           
                var result = await masterBL.GetAdminService<SrvDropDownTypeBL>().DeleteDropDownType(Id);
                return Ok(result);
           
        }

    }
}
