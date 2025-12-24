using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Calendars;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.EvalResult;
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

    public class DepEvalMatrixController : Controller
    {

        private readonly UserInfo userInfoSession;
        private readonly MasterBL masterBL;
        private readonly IHttpContextAccessor httpContextAccessor;

        public DepEvalMatrixController(UserInfo userInfoSession, MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
        {

            this.userInfoSession = userInfoSession;
            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;

        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_DEP_EVAL_MATRIX })]
        public async Task<IActionResult> Index()
        {
            var model = new DepEvalMatrixVM(httpContextAccessor);
            await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminDepEvalMatrix },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_DEP_EVAL_MATRIX },
                new string[] {
                 ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT
            });

            var property = typeof(DepEvalMatrix).GetProperty("OrderNo");
            model.containsOrderNo = property != null ? true : false;
            return View(model);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_DEP_EVAL_MATRIX })]
        public async Task<IActionResult> GetAllDepEvalMatrix(int Page = 1)
        {
            var PageSize = Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
            var response = await masterBL.GetAdminService<SrvDepEvalMatrixBL>().GetDepEvalMatrixList(Page, PageSize);
            return Ok(response);
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_DEP_EVAL_MATRIX })]
        public async Task<IActionResult> SaveDepEvalMatrix()
        {
            var result = new DepEvalMatrixDTO();
            var model = Request.Form["request"][0]?.StringToObject<DepEvalMatrixDTO>();

            bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(model!, ConstantKeys.AdminPermission.ADD_ADMIN_DEP_EVAL_MATRIX);
            if (validateObject)
            {
                result = await masterBL.GetAdminService<SrvDepEvalMatrixBL>().SaveDepEvalMatrix(model!);
            }
            return Ok(new ResponseEntity(result));

        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_DEP_EVAL_MATRIX })]
        public async Task<IActionResult> UpdateDepEvalMatrix()
        {
            var result = new DepEvalMatrixDTO();
            var model = Request.Form["request"][0]?.StringToObject<DepEvalMatrixDTO>();

            bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(model!, ConstantKeys.AdminPermission.ADD_ADMIN_DEP_EVAL_MATRIX);
            if (validateObject)
            {
                result = await masterBL.GetAdminService<SrvDepEvalMatrixBL>().UpdateDepEvalMatrix(model!);
            }
            return Ok(new ResponseEntity(result));

        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_DEP_EVAL_MATRIX })]
        public async Task<IActionResult> DeleteDepEvalMatrix(Guid Id)
        {

            var result = await masterBL.GetAdminService<SrvDepEvalMatrixBL>().DeleteDepEvalMatrix(Id);
            return Ok(result);

        }
    }
}
