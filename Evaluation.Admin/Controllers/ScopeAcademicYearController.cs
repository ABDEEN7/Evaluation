using Azure.Core;
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
    public class ScopeAcademicYearController : Controller
    {

        private readonly UserInfo userInfoSession;
        private readonly MasterBL masterBL;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ScopeAcademicYearController(UserInfo userInfoSession, MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
        {

            this.userInfoSession = userInfoSession;
            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;

        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_SCOPE_ACADEMIC_YEAR_SCOPE })]
        public async Task<IActionResult> Index()
        {
            var model = new ScopeAcademicYearVM(httpContextAccessor);
            await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminScopeAcademicYear },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_SCOPE_ACADEMIC_YEAR_SCOPE },
                new string[] {
                 ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT
            });

            var property = typeof(ScopeAcademicYear).GetProperty("OrderNo");
            model.containsOrderNo = property != null ? true : false;
            return View(model);
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_SCOPE_ACADEMIC_YEAR_SCOPE })]
        public async Task<IActionResult> GetAllScopeAcademicYear([FromBody] ScopeFormItemRequest request)
        {
            var PageSize = Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
            var response = await masterBL.GetAdminService<SrvScopeAcademicYearBL>().GetScopeAcademicYearList(request, PageSize);
            return Ok(response);
        }


        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_SCOPE_ACADEMIC_YEAR_SCOPE })]
        public async Task<IActionResult> SaveScopeAcademicYear()
        {

            var request = Request.Form["request"][0]?.StringToObject<ScopeAcademicYearDTO>();
            var result = new ScopeAcademicYearDTO();
            bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_SCOPE_ACADEMIC_YEAR_SCOPE);
            if (validateObject)
            {
                result = await masterBL.GetAdminService<SrvScopeAcademicYearBL>().SaveScopeAcademicYear(request!);

            }
            return Ok(new ResponseEntity(result));

        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_SCOPE_ACADEMIC_YEAR_SCOPE })]
        public async Task<IActionResult> UpdateScopeAcademicYear()
        {

            var request = Request.Form["request"][0]?.StringToObject<ScopeAcademicYearDTO>();
            var result = new ScopeAcademicYearDTO();
            bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_SCOPE_ACADEMIC_YEAR_SCOPE);
            if (validateObject)
            {
                result = await masterBL.GetAdminService<SrvScopeAcademicYearBL>().UpdateScopeAcademicYear(request!);
            }
            return Ok(new ResponseEntity(result));

        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_SCOPE_ACADEMIC_YEAR_SCOPE })]
        public async Task<IActionResult> DeleteScopeAcademicYear(Guid Id)
        {

            var result = await masterBL.GetAdminService<SrvScopeAcademicYearBL>().DeleteScopeAcademicYear(Id);
            return Ok(result);

        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_SCOPE_ACADEMIC_YEAR_SCOPE })]
        public async Task<IActionResult> UpdateDepartmentOrder()
        {

            var model = Request.Form["OrderObj"][0]?.StringToObject<List<OrderingDTO>>();
            var result = await masterBL.GetAdminService<SrvScopeAcademicYearBL>().UpdateDepartmentOrder(model!);
            return Ok(result);

        }

    }
}
