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
    public class PlanStatusesController : Controller
    {
        
        private readonly UserInfo userInfoSession;
        private readonly MasterBL masterBL;
        private readonly IHttpContextAccessor httpContextAccessor;

        public PlanStatusesController(UserInfo userInfoSession, MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
        {

            this.userInfoSession = userInfoSession;
            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;
           
        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_PLANSTATUS })]
        public async Task<IActionResult> Index()
        {
            var model = new PlanStatusesVM(httpContextAccessor);
            await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminPlanStatuses },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_PLANSTATUS },
                new string[] {
                 ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT
            });

            var property = typeof(PlanStatus).GetProperty("OrderNo");
            model.containsOrderNo = property != null ? true : false;
            return View(model);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_PLANSTATUS })]
        public async Task<IActionResult> GetAllPlanStatuses(int Page = 1)
        {
            var PageSize =  Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
            var response = await masterBL.GetAdminService<SrvPlanStatusesBL>().GetPlanStatusesList(Page, PageSize);
            return Ok(response);
        }


        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_PLANSTATUS })]
        public async Task<IActionResult> SavePlanStatuses()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<PlanStatusesDTO>();
            var result = new PlanStatusesDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_PLANSTATUS);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvPlanStatusesBL>().SavePlanStatuses(request!);
                    
                }
                return Ok(result);
           
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_PLANSTATUS })]
        public async Task<IActionResult> UpdatePlanStatuses()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<PlanStatusesDTO>();
            var result = new PlanStatusesDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_PLANSTATUS);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvPlanStatusesBL>().UpdatePlanStatuses(request!);
                }
                return Ok(result);
            
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_PLANSTATUS })]
        public async Task<IActionResult> UpdatePlanStatusesOrder()
        {

            var model = Request.Form["OrderObj"][0]?.StringToObject<List<OrderingDTO>>();
            var result = await masterBL.GetAdminService<SrvPlanStatusesBL>().UpdatePlanStatusesOrder(model!);
            return Ok(result);

        }

        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_PLANSTATUS })]
        public async Task<IActionResult> DeletePlanStatuses(Guid Id)
        {
           
                var result = await masterBL.GetAdminService<SrvPlanStatusesBL>().DeletePlanStatuses(Id);
                return Ok(result);
           
        }

    }
}
