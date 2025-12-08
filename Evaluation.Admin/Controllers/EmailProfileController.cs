using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.DAL.Models.SystemSetting;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.Models.Admin;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Admin.Controllers
{
    [Authorize]
    public class EmailProfileController : Controller
    {

        private readonly MasterBL masterBL;

        private readonly IHttpContextAccessor httpContextAccessor;
        public EmailProfileController(MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
        {


            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;

        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_EMAILPROFILE })]
        public async Task<IActionResult> Index()
        {
            var model = new EmailProfileVM(httpContextAccessor);
            await model.LoadAllAData(
                new string[] { ConstantKeys.AdminPages.AdminEmailProfile },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_EMAILPROFILE }
                );
           
            var property = typeof(EmailProfile).GetProperty("OrderNo");
            model.containsOrderNo = property != null ? true : false;
            return View(model);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_EMAILPROFILE })]
        public async Task<IActionResult> GetAllEmailProfile(int Page = 1)
        {
            var PageSize =  Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
            var response = await masterBL.GetAdminService<SrvEmailProfileBL>().GetEmailProfileList(Page, PageSize);
            return Ok(response);
        }

        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_EMAILPROFILE })]
        public async Task<IActionResult> GetDefaultEmailProfile()
        {
           
            var response = await masterBL.GetAdminService<SrvEmailProfileBL>().GetDefaultEmailProfile();
            return Ok(response);
        }

        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_EMAILPROFILE })]
        public async Task<IActionResult> SaveEmailProfile()
        {

            var request = Request.Form["request"][0]?.StringToObject<EmailProfileDTO>();
            var result = new EmailProfileDTO();
            bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_EMAILPROFILE);
            if (validateObject)
            {
                result = await masterBL.GetAdminService<SrvEmailProfileBL>().SaveEmailProfile(request!);

            }
            return Ok(result);
        }


        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_EMAILPROFILE })]
        public async Task<IActionResult> UpdateEmailProfile()
        {
            var request = Request.Form["request"][0]?.StringToObject<EmailProfileDTO>();
            var result = new EmailProfileDTO();
            bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_EMAILPROFILE);
            if (validateObject)
            {
                result = await masterBL.GetAdminService<SrvEmailProfileBL>().UpdateEmailProfile(request!);

            }
            return Ok(result);
        }


        [HttpGet()]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_EMAILPROFILE })]
        public async Task<IActionResult> DeleteEmailProfile(Guid id)
        {
            //throw new NotImplementedException();    
            var result = await masterBL.GetAdminService<SrvEmailProfileBL>().DeleteEmailProfile(id);
            return Ok(result);
        }


    }
}
