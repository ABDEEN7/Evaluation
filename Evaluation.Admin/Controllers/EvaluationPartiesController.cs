using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.FormsModules;
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
    public class EvaluationPartiesController : Controller
    {
        
        private readonly UserInfo userInfoSession;
        private readonly MasterBL masterBL;
        private readonly IHttpContextAccessor httpContextAccessor;

        public EvaluationPartiesController(UserInfo userInfoSession, MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
        {

            this.userInfoSession = userInfoSession;
            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;
           
        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_EVALUATIONPARTIES })]
        public async Task<IActionResult> Index()
        {
            var model = new EvaluationPartiesVM(httpContextAccessor);
            await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminEvaluationParties },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_EVALUATIONPARTIES },
                new string[] {
                 ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT
            });

            var property = typeof(EvaluationParty).GetProperty("OrderNo");
            model.containsOrderNo = property != null ? true : false;
            return View(model);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_EVALUATIONPARTIES })]
        public async Task<IActionResult> GetAllEvaluationParties(int Page = 1)
        {
            var PageSize =  Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
            var response = await masterBL.GetAdminService<SrvEvaluationPartiesBL>().GetEvaluationPartiesList(Page, PageSize);
            return Ok(response);
        }


        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_EVALUATIONPARTIES })]
        public async Task<IActionResult> SaveEvaluationParties()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<EvaluationPartiesDTO>();
            var result = new EvaluationPartiesDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_EVALUATIONPARTIES);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvEvaluationPartiesBL>().SaveEvaluationParties(request!);
                    
                }
                return Ok(result);
           
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_EVALUATIONPARTIES })]
        public async Task<IActionResult> UpdateEvaluationParties()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<EvaluationPartiesDTO>();
            var result = new EvaluationPartiesDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_EVALUATIONPARTIES);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvEvaluationPartiesBL>().UpdateEvaluationParties(request!);
                }
                return Ok(result);
            
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_EVALUATIONPARTIES })]
        public async Task<IActionResult> UpdateEvaluationPartiesOrder()
        {

            var model = Request.Form["OrderObj"][0]?.StringToObject<List<OrderingDTO>>();
            var result = await masterBL.GetAdminService<SrvEvaluationPartiesBL>().UpdateEvaluationPartiesOrder(model!);
            return Ok(result);

        }

        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_EVALUATIONPARTIES })]
        public async Task<IActionResult> DeleteEvaluationParties(Guid Id)
        {
           
                var result = await masterBL.GetAdminService<SrvEvaluationPartiesBL>().DeleteEvaluationParties(Id);
                return Ok(result);
           
        }

    }
}
