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
    public class AcademicYearScopeController : Controller
    {
        
        private readonly UserInfo userInfoSession;
        private readonly MasterBL masterBL;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AcademicYearScopeController(UserInfo userInfoSession, MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
        {

            this.userInfoSession = userInfoSession;
            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;
           
        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_ACADEMIC_YEAR_SCOPE })]
        public async Task<IActionResult> Index()
        {
            var model = new AcademicYearScopeVM(httpContextAccessor);
            await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminAcademicYearScope },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_ACADEMIC_YEAR_SCOPE },
                new string[] {
                 ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT
            });

            var property = typeof(AcademicYearScope).GetProperty("OrderNo");
            model.containsOrderNo = property != null ? true : false;
            return View(model);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_ACADEMIC_YEAR_SCOPE })]
        public async Task<IActionResult> GetAllAcademicYearScope(int Page = 1)
        {
            var PageSize =  Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
            var response = await masterBL.GetAdminService<SrvAcademicYearScopeBL>().GetAcademicYearScopeList(Page, PageSize);
            return Ok(response);
        }


        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_ACADEMIC_YEAR_SCOPE })]
        public async Task<IActionResult> SaveAcademicYearScope()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<AcademicYearScopeDTO>();
                var result = new AcademicYearScopeDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_ACADEMIC_YEAR_SCOPE);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvAcademicYearScopeBL>().SaveAcademicYearScope(request!);
                    
                }
                return Ok(new ResponseEntity(result));
           
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_ACADEMIC_YEAR_SCOPE })]
        public async Task<IActionResult> UpdateAcademicYearScope()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<AcademicYearScopeDTO>();
                var result = new AcademicYearScopeDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_ACADEMIC_YEAR_SCOPE);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvAcademicYearScopeBL>().UpdateAcademicYearScope(request!);
                }
                return Ok(new ResponseEntity(result));
            
        }

        
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_ACADEMIC_YEAR_SCOPE })]
        public async Task<IActionResult> DeleteAcademicYearScope(Guid Id)
        {
           
                var result = await masterBL.GetAdminService<SrvAcademicYearScopeBL>().DeleteAcademicYearScope(Id);
                return Ok(result);
           
        }

    }
}
