using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Planing;
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
    public class OrgTypesController : Controller
    {
        
        private readonly UserInfo userInfoSession;
        private readonly MasterBL masterBL;
        private readonly IHttpContextAccessor httpContextAccessor;

        public OrgTypesController(UserInfo userInfoSession, MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
        {

            this.userInfoSession = userInfoSession;
            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;
           
        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_ORGTYPES })]
        public async Task<IActionResult> Index()
        {
            var model = new OrgTypesVM(httpContextAccessor);
            await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminOrgTypes },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_ORGTYPES },
                new string[] {
                 ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT
            });

            var property = typeof(OrgType).GetProperty("OrderNo");
            model.containsOrderNo = property != null ? true : false;
            return View(model);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_ORGTYPES })]
        public async Task<IActionResult> GetAllOrgTypes(int Page = 1)
        {
            var PageSize =  Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
            var response = await masterBL.GetAdminService<SrvOrgTypesBL>().GetOrgTypesList(Page, PageSize);
            return Ok(response);
        }


        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_ORGTYPES })]
        public async Task<IActionResult> SaveOrgTypes()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<OrgTypesDTO>();
            var result = new OrgTypesDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_ORGTYPES);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvOrgTypesBL>().SaveOrgTypes(request!);
                    
                }
                return Ok(result);
           
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_ORGTYPES })]
        public async Task<IActionResult> UpdateOrgTypes()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<OrgTypesDTO>();
            var result = new OrgTypesDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_ORGTYPES);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvOrgTypesBL>().UpdateOrgTypes(request!);
                }
                return Ok(result);
            
        }

        
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_ORGTYPES })]
        public async Task<IActionResult> DeleteOrgTypes(Guid Id)
        {
           
                var result = await masterBL.GetAdminService<SrvOrgTypesBL>().DeleteOrgTypes(Id);
                return Ok(result);
           
        }

    }
}
