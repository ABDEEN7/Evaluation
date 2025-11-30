using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Calendars;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.Models.Admin;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Admin.Controllers
{
    [Authorize]
    public class AcademicYearController : Controller
    {
        
        private readonly UserInfo userInfoSession;
        private readonly MasterBL masterBL;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AcademicYearController(UserInfo userInfoSession, MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
        {

            this.userInfoSession = userInfoSession;
            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;
           
        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_ACADEMIC_YEAR })]
        public async Task<IActionResult> Index()
        {
            var model = new AcademicYearVM(httpContextAccessor);
            await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminAcademicYear },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_ACADEMIC_YEAR },
                new string[] {
                 ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT
            });

            var property = typeof(AcademicYear).GetProperty("OrderNo");
            model.containsOrderNo = property != null ? true : false;
            return View(model);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_ACADEMIC_YEAR })]
        public async Task<IActionResult> GetAllAcademicYear(int Page = 1)
        {
            var PageSize =  Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
            var response = await masterBL.GetAdminService<SrvAcademicYearBL>().GetAcademicYearList(Page, PageSize);
            return Ok(response);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_ACADEMIC_YEAR })]
        public async Task<IActionResult> GetCurrentAcademicYear(Guid DepartmentId)
        {

            var response = await masterBL.GetAdminService<SrvAcademicYearBL>().GetCurrentAcademicYear(DepartmentId);
            return Ok(response);
        }

        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_ACADEMIC_YEAR })]
        public async Task<IActionResult> SaveAcademicYear()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<AcademicYearDTO>();
                var result = new AcademicYearDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_ACADEMIC_YEAR);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvAcademicYearBL>().SaveAcademicYear(request!);
                    
                }
                return Ok(new ResponseEntity(result));
           
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_ACADEMIC_YEAR })]
        public async Task<IActionResult> UpdateAcademicYear()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<AcademicYearDTO>();
                var result = new AcademicYearDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_ACADEMIC_YEAR);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvAcademicYearBL>().UpdateAcademicYear(request!);
                }
                return Ok(new ResponseEntity(result));
            
        }

        
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_ACADEMIC_YEAR })]
        public async Task<IActionResult> DeleteAcademicYear(Guid Id)
        {
           
                var result = await masterBL.GetAdminService<SrvAcademicYearBL>().DeleteAcademicYear(Id);
                return Ok(result);
           
        }

    }
}
