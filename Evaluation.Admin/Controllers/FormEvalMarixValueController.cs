using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Models.Master;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.Models.Admin;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Evaluation.Admin.Controllers
{
    [Authorize]
    public class FormEvalMarixValueController : Controller
    {
        
        private readonly UserInfo userInfoSession;
        private readonly MasterBL masterBL;
        private readonly IHttpContextAccessor httpContextAccessor;

        public FormEvalMarixValueController(UserInfo userInfoSession, MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
        {

            this.userInfoSession = userInfoSession;
            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;
           
        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_FORMEVAL_MATRIX_VALUE })]
        public async Task<IActionResult> Index()
        {
            var model = new FormEvalMarixValueVM(httpContextAccessor);
            await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminFormEvalMarixValue },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_FORMEVAL_MATRIX_VALUE },
                new string[] {
                 ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT
            });

            var property = typeof(FormEvalMarixValue).GetProperty("OrderNo");
            model.containsOrderNo = property != null ? true : false;
            return View(model);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_FORMEVAL_MATRIX_VALUE })]
        public async Task<IActionResult> GetAllFormEvalMarixValue(int Page = 1)
        {
            var PageSize =  Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
            var response = await masterBL.GetAdminService<SrvFormEvalMarixValueBL>().GetFormEvalMarixValueList(Page, PageSize);
            return Ok(response);
        }


        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_FORMEVAL_MATRIX_VALUE })]
        public async Task<IActionResult> SaveFormEvalMarixValue()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<FormEvalMarixValueDTO>();
            var result = new FormEvalMarixValueDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_FORMEVAL_MATRIX_VALUE);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvFormEvalMarixValueBL>().SaveFormEvalMarixValue(request!);
                    
                }
                return Ok(result);
           
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_FORMEVAL_MATRIX_VALUE })]
        public async Task<IActionResult> UpdateFormEvalMarixValue()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<FormEvalMarixValueDTO>();
            var result = new FormEvalMarixValueDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_FORMEVAL_MATRIX_VALUE);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvFormEvalMarixValueBL>().UpdateFormEvalMarixValue(request!);
                }
                return Ok(result);
            
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_FORMEVAL_MATRIX_VALUE })]
        public async Task<IActionResult> UpdateFormEvalMarixValueOrder()
        {

            var model = Request.Form["OrderObj"][0]?.StringToObject<List<OrderingDTO>>();
            var result = await masterBL.GetAdminService<SrvFormEvalMarixValueBL>().UpdateFormEvalMarixValueOrder(model!);
            return Ok(result);

        }

        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_FORMEVAL_MATRIX_VALUE })]
        public async Task<IActionResult> DeleteFormEvalMarixValue(Guid Id)
        {
           
                var result = await masterBL.GetAdminService<SrvFormEvalMarixValueBL>().DeleteFormEvalMarixValue(Id);
                return Ok(result);
           
        }

    }
}
