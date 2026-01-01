using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.Models.Admin;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Evaluation.Admin.Controllers
{
    [Authorize]
    public class SchoolsController : Controller
    {
        
        private readonly UserInfo userInfoSession;
        private readonly MasterBL masterBL;
        private readonly IHttpContextAccessor httpContextAccessor;

        public SchoolsController(UserInfo userInfoSession, MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
        {

            this.userInfoSession = userInfoSession;
            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;
           
        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_SCHOOL })]
        public async Task<IActionResult> Index()
        {
            var model = new SchoolsVM(httpContextAccessor);
            await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminSchools },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_SCHOOL },
                new string[] {
                 ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT
            });
           
            var property = typeof(School).GetProperty("OrderNo");
            model.containsOrderNo = property != null ? true : false;
            return View(model);
        }

        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_SCHOOL })]
        public async Task<IActionResult> GetOrgClass()
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            var OrgClass = await masterBL.GetAdminService<SrvSchoolsBL>().GetOrgClass();
            response.Add("OrgClass", OrgClass);
            return Ok(new ResponseEntity(response));
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_SCHOOL })]
       
        public async Task<IActionResult> GetAllSchools([FromBody] AdminSearchDTO request)
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
            var response = await masterBL.GetAdminService<SrvSchoolsBL>().GetSchoolsList(request);
            return Ok(response);
        }


        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_SCHOOL })]
        public async Task<IActionResult> SaveSchools()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<SchoolsDTO>();

            var result = new SchoolsDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_SCHOOL);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvSchoolsBL>().SaveSchools(request!);
                    
                }
                return Ok(new ResponseEntity(result));
           
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_SCHOOL })]
        public async Task<IActionResult> UpdateSchools()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<SchoolsDTO>();
          
            var result = new SchoolsDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_SCHOOL);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvSchoolsBL>().UpdateSchools(request!);
                }
                return Ok(new ResponseEntity(result));
            
        }

       
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_SCHOOL })]
        public async Task<IActionResult> DeleteSchools(Guid Id)
        {
           
                var result = await masterBL.GetAdminService<SrvSchoolsBL>().DeleteSchools(Id);
                return Ok(result);
           
        }

    }
}
