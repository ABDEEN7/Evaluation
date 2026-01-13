using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Master;
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
    public class EvaluationTypeController : Controller
    {
        
        private readonly UserInfo userInfoSession;
        private readonly MasterBL masterBL;
        private readonly IHttpContextAccessor httpContextAccessor;

        public EvaluationTypeController(UserInfo userInfoSession, MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
        {

            this.userInfoSession = userInfoSession;
            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;
           
        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_EVALUATIONTYPE })]
        public async Task<IActionResult> Index()
        {
            var model = new EvaluationTypeVM(httpContextAccessor);
            await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminEvaluationType },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_EVALUATIONTYPE },
                new string[] {
                 ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT
            });

            var property = typeof(EvaluationType).GetProperty("OrderNo");
            model.containsOrderNo = property != null ? true : false;
            return View(model);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_EVALUATIONTYPE })]
        public async Task<IActionResult> GetAllEvaluationType(int Page = 1)
        {
            var PageSize =  Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
            var response = await masterBL.GetAdminService<SrvEvaluationTypeBL>().GetEvaluationTypeList(Page, PageSize);
            return Ok(response);
        }


        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_EVALUATIONTYPE })]
        public async Task<IActionResult> SaveEvaluationType()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<EvaluationTypeDTO>();
            var result = new EvaluationTypeDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_EVALUATIONTYPE);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvEvaluationTypeBL>().SaveEvaluationType(request!);
                    
                }
                return Ok(result);
           
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_EVALUATIONTYPE })]
        public async Task<IActionResult> UpdateEvaluationType()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<EvaluationTypeDTO>();
            var result = new EvaluationTypeDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_EVALUATIONTYPE);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvEvaluationTypeBL>().UpdateEvaluationType(request!);
                }
                return Ok(result);
            
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_EVALUATIONTYPE })]
        public async Task<IActionResult> UpdateEvaluationTypeOrder()
        {

            var model = Request.Form["OrderObj"][0]?.StringToObject<List<OrderingDTO>>();
            var result = await masterBL.GetAdminService<SrvEvaluationTypeBL>().UpdateEvaluationTypeOrder(model!);
            return Ok(result);

        }

        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_EVALUATIONTYPE })]
        public async Task<IActionResult> DeleteEvaluationType(Guid Id)
        {
           
                var result = await masterBL.GetAdminService<SrvEvaluationTypeBL>().DeleteEvaluationType(Id);
                return Ok(result);
           
        }

    }
}
