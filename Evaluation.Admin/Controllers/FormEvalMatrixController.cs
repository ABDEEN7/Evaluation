using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Calendars;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.Admin;
using Evaluation.Services.Models.Admin;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Admin.Controllers
{
    [Authorize]

    public class FormEvalMatrixController : Controller
    {

        private readonly UserInfo userInfoSession;
        private readonly MasterBL masterBL;
        private readonly IHttpContextAccessor httpContextAccessor;

        public FormEvalMatrixController(UserInfo userInfoSession, MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
        {

            this.userInfoSession = userInfoSession;
            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;

        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_FORM_EVAL_MATRIX })]
        public async Task<IActionResult> Index()
        {
            var model = new FormEvalMatrixVM(httpContextAccessor);
            await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminFormEvalMatrix },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_FORM_EVAL_MATRIX },
                new string[] {
                 ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT
            });

            var property = typeof(FormEvalMatrix).GetProperty("OrderNo");
            model.containsOrderNo = property != null ? true : false;
            return View(model);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_FORM_EVAL_MATRIX })]
        public async Task<IActionResult> GetAllFormEvalMatrix(int Page = 1)
        {
            var PageSize = Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
            var response = await masterBL.GetAdminService<SrvFormEvalMatrixBL>().GetFormEvalMatrixList(Page, PageSize);
            return Ok(response);
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_FORM_EVAL_MATRIX })]
        public async Task<IActionResult> SaveFormEvalMatrix()
        {

            var files = Request.Form.Files;
            var result = new FormEvalMatrixDTO();
            var model = Request.Form["request"][0]?.StringToObject<FormEvalMatrixDTO>();
           
            bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(model!, ConstantKeys.AdminPermission.ADD_ADMIN_FORM_EVAL_MATRIX);
            if (validateObject)
            {
                result = await masterBL.GetAdminService<SrvFormEvalMatrixBL>().SaveFormEvalMatrix(model!);
            }
            return Ok(new ResponseEntity(result));

        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_FORM_EVAL_MATRIX })]
        public async Task<IActionResult> UpdateFormEvalMatrix()
        {

            var files = Request.Form.Files;
            var result = new FormEvalMatrixDTO();
            var model = Request.Form["request"][0]?.StringToObject<FormEvalMatrixDTO>();
          
            bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(model!, ConstantKeys.AdminPermission.ADD_ADMIN_FORM_EVAL_MATRIX);
            if (validateObject)
            {
                result = await masterBL.GetAdminService<SrvFormEvalMatrixBL>().UpdateFormEvalMatrix(model!);
            }
            return Ok(new ResponseEntity(result));

        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_FORM_EVAL_MATRIX })]
        public async Task<IActionResult> UpdateFormEvalMatrixOrder()
        {

            var model = Request.Form["OrderObj"][0]?.StringToObject<List<OrderingDTO>>();
            var result = await masterBL.GetAdminService<SrvFormEvalMatrixBL>().UpdateFormEvalMatrixOrder(model!);
            return Ok(result);

        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_FORM_EVAL_MATRIX })]
        public async Task<IActionResult> HasRequiredFollowUp(Guid formEvalMatrixId)
        {
            var result = await masterBL.GetAdminService<SrvFormEvalMatrixBL>().HasRequiredFollowUpAsync(formEvalMatrixId);
            return Ok(result);
        }
        
    }
}
