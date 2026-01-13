using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.DAL.Helper;
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
    public class SchoolLevelController : Controller
    {
        
        private readonly UserInfo userInfoSession;
        private readonly MasterBL masterBL;
        private readonly IHttpContextAccessor httpContextAccessor;

        public SchoolLevelController(UserInfo userInfoSession, MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
        {

            this.userInfoSession = userInfoSession;
            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;
           
        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_SCHOOLLEVEL })]
        public async Task<IActionResult> Index()
        {
            var model = new SchoolLevelVM(httpContextAccessor);
            await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminSchoolLevel },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_SCHOOLLEVEL },
                new string[] {
                 ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT
            });

            var property = typeof(SchoolLevel).GetProperty("OrderNo");
            model.containsOrderNo = property != null ? true : false;
            return View(model);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_SCHOOLLEVEL })]
        public async Task<IActionResult> GetAllSchoolLevel(int Page = 1)
        {
            var PageSize =  Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
            var response = await masterBL.GetAdminService<SrvSchoolLevelBL>().GetSchoolLevelList(Page, PageSize);
            return Ok(response);
        }


        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_SCHOOLLEVEL })]
        public async Task<IActionResult> SaveSchoolLevel()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<SchoolLevelDTO>();
            var result = new SchoolLevelDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_SCHOOLLEVEL);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvSchoolLevelBL>().SaveSchoolLevel(request!);
                    
                }
                return Ok(result);
           
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_SCHOOLLEVEL })]
        public async Task<IActionResult> UpdateSchoolLevel()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<SchoolLevelDTO>();
            var result = new SchoolLevelDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_SCHOOLLEVEL);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvSchoolLevelBL>().UpdateSchoolLevel(request!);
                }
                return Ok(result);
            
        }


        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_SCHOOLLEVEL })]
        public async Task<IActionResult> DeleteSchoolLevel(Guid Id)
        {
           
                var result = await masterBL.GetAdminService<SrvSchoolLevelBL>().DeleteSchoolLevel(Id);
                return Ok(result);
           
        }

    }
}
