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
    public class EvalFormTypeController : Controller
    {
        
        private readonly UserInfo userInfoSession;
        private readonly MasterBL masterBL;
        private readonly IHttpContextAccessor httpContextAccessor;

        public EvalFormTypeController(UserInfo userInfoSession, MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
        {

            this.userInfoSession = userInfoSession;
            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;
           
        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_EVALFORMTYPE })]
        public async Task<IActionResult> Index()
        {
            var model = new EvalFormTypeVM(httpContextAccessor);
            await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminEvalFormType },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_EVALFORMTYPE },
                new string[] {
                 ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT
            });

            var property = typeof(EvalFormType).GetProperty("OrderNo");
            model.containsOrderNo = property != null ? true : false;
            return View(model);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_EVALFORMTYPE })]
        public async Task<IActionResult> GetAllEvalFormType(int Page = 1)
        {
            var PageSize =  Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
            var response = await masterBL.GetAdminService<SrvEvalFormTypeBL>().GetEvalFormTypeList(Page, PageSize);
            return Ok(response);
        }


        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_EVALFORMTYPE })]
        public async Task<IActionResult> SaveEvalFormType()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<EvalFormTypeDTO>();
            var result = new EvalFormTypeDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_EVALFORMTYPE);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvEvalFormTypeBL>().SaveEvalFormType(request!);
                    
                }
                return Ok(result);
           
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_EVALFORMTYPE })]
        public async Task<IActionResult> UpdateEvalFormType()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<EvalFormTypeDTO>();
            var result = new EvalFormTypeDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_EVALFORMTYPE);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvEvalFormTypeBL>().UpdateEvalFormType(request!);
                }
                return Ok(result);
            
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_EVALFORMTYPE })]
        public async Task<IActionResult> UpdateEvalFormTypeOrder()
        {

            var model = Request.Form["OrderObj"][0]?.StringToObject<List<OrderingDTO>>();
            var result = await masterBL.GetAdminService<SrvEvalFormTypeBL>().UpdateEvalFormTypeOrder(model!);
            return Ok(result);

        }

        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_EVALFORMTYPE })]
        public async Task<IActionResult> DeleteEvalFormType(Guid Id)
        {
           
                var result = await masterBL.GetAdminService<SrvEvalFormTypeBL>().DeleteEvalFormType(Id);
                return Ok(result);
           
        }

    }
}
