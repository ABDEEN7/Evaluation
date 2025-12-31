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
    public class EducationLevelController : Controller
    {
        
        private readonly UserInfo userInfoSession;
        private readonly MasterBL masterBL;
        private readonly IHttpContextAccessor httpContextAccessor;

        public EducationLevelController(UserInfo userInfoSession, MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
        {

            this.userInfoSession = userInfoSession;
            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;
           
        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_EDUCATIONLEVEL })]
        public async Task<IActionResult> Index()
        {
            var model = new EducationLevelVM(httpContextAccessor);
            await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminEducationLevel },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_EDUCATIONLEVEL },
                new string[] {
                 ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT
            });

            var property = typeof(EducationLevel).GetProperty("OrderNo");
            model.containsOrderNo = property != null ? true : false;
            return View(model);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_EDUCATIONLEVEL })]
        public async Task<IActionResult> GetAllEducationLevel(int Page = 1)
        {
            var PageSize =  Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
            var response = await masterBL.GetAdminService<SrvEducationLevelBL>().GetEducationLevelList(Page, PageSize);
            return Ok(response);
        }


        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_EDUCATIONLEVEL })]
        public async Task<IActionResult> SaveEducationLevel()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<EducationLevelDTO>();
            var result = new EducationLevelDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_EDUCATIONLEVEL);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvEducationLevelBL>().SaveEducationLevel(request!);
                    
                }
                return Ok(result);
           
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_EDUCATIONLEVEL })]
        public async Task<IActionResult> UpdateEducationLevel()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<EducationLevelDTO>();
            var result = new EducationLevelDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_EDUCATIONLEVEL);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvEducationLevelBL>().UpdateEducationLevel(request!);
                }
                return Ok(result);
            
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_EDUCATIONLEVEL })]
        public async Task<IActionResult> UpdateEducationLevelOrder()
        {

            var model = Request.Form["OrderObj"][0]?.StringToObject<List<OrderingDTO>>();
            var result = await masterBL.GetAdminService<SrvEducationLevelBL>().UpdateEducationLevelOrder(model!);
            return Ok(result);

        }

        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_EDUCATIONLEVEL })]
        public async Task<IActionResult> DeleteEducationLevel(Guid Id)
        {
           
                var result = await masterBL.GetAdminService<SrvEducationLevelBL>().DeleteEducationLevel(Id);
                return Ok(result);
           
        }

    }
}
