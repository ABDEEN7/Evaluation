using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Models.SystemSetting;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.Models.Admin;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Evaluation.SharedHelper.Enums.ConstantKeys;

namespace Evaluation.Admin.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {

        private readonly UserInfo userInfoSession;
        private readonly MasterBL masterBL;
        private readonly IHttpContextAccessor httpContextAccessor;

        public DepartmentController(UserInfo userInfoSession, MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
        {

            this.userInfoSession = userInfoSession;
            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;

        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_DEPARTMENT })]
        public async Task<IActionResult> Index()
        {
            var model = new DepartmentVM(httpContextAccessor);
            await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminDepartment },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_DEPARTMENT },
                new string[] {
                 ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT
            });

            var property = typeof(Department).GetProperty("OrderNo");
            model.containsOrderNo = property != null ? true : false;
            return View(model);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_DEPARTMENT })]
        public async Task<IActionResult> GetAllDepartment(int Page = 1)
        {
            var PageSize = Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
            var response = await masterBL.GetAdminService<SrvDepartmentBL>().GetDepartmentList(Page, PageSize);
            return Ok(response);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_DEPARTMENT })]
        public async Task<IActionResult> GetAttachmentById(Guid Id)
        {

            var result = await masterBL.GetAdminService<SrvTemplateDocumentBL>().GetAttachment(Id);
            return Ok(result);

        }

        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_DEPARTMENT })]
        public async Task<IActionResult> SaveDepartment()
        {

            var files = Request.Form.Files;
            var result = new DepartmentDTO();
            var model = Request.Form["request"][0]?.StringToObject<DepartmentDTO>();
            List<WebsiteAttachmentDTO> filemodel = new List<WebsiteAttachmentDTO>();
            if (null != files)
            {
                var FinalFiles = files.Where(c => c.Length > 0).ToList();
                if (FinalFiles.Count > 0)
                {
                    var constraintList = await masterBL.GetAdminService<SrvSystemSettingBL>().GetAppConstraints(ConstantKeys.AdminPermission.ADD_ADMIN_DEPARTMENT);

                    var response = await masterBL.GetAdminService<SrvBaseBL>().ValidateWebsiteAttachment(constraintList, FinalFiles);
                    if (response != null)
                    {
                        if (response.ResponseStatus == true)
                        {
                            filemodel = response.Data!;

                        }
                        else
                        {
                            model.ResponseMessage = response.ResponseMessage;

                            model.ResponseState = response.ResponseStatus;

                            return Json(new ResponseEntity(model));
                        }
                    }

                }


            }

            bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(model!, ConstantKeys.AdminPermission.ADD_ADMIN_DEPARTMENT);
            if (validateObject)
            {
                result = await masterBL.GetAdminService<SrvDepartmentBL>().SaveDepartment(model!, filemodel);
            }
            return Ok(new ResponseEntity(result));

        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_DEPARTMENT })]
        public async Task<IActionResult> UpdateDepartment()
        {

            var files = Request.Form.Files;
            var result = new DepartmentDTO();
            var model = Request.Form["request"][0]?.StringToObject<DepartmentDTO>();
            List<WebsiteAttachmentDTO> filemodel = new List<WebsiteAttachmentDTO>();
            if (null != files)
            {
                var FinalFiles = files.Where(c => c.Length > 0).ToList();
                if (FinalFiles.Count > 0)
                {
                    var constraintList = await masterBL.GetAdminService<SrvSystemSettingBL>().GetAppConstraints(ConstantKeys.AdminPermission.ADD_ADMIN_DEPARTMENT);

                    var response = await masterBL.GetAdminService<SrvBaseBL>().ValidateWebsiteAttachment(constraintList, FinalFiles);
                    if (response != null)
                    {
                        if (response.ResponseStatus == true)
                        {
                            filemodel = response.Data!;

                        }
                        else
                        {
                            model.ResponseMessage = response.ResponseMessage;

                            model.ResponseState = response.ResponseStatus;

                            return Json(new ResponseEntity(model));
                        }
                    }

                }


            }

            bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(model!, ConstantKeys.AdminPermission.ADD_ADMIN_DEPARTMENT);
            if (validateObject)
            {
                result = await masterBL.GetAdminService<SrvDepartmentBL>().UpdateDepartment(model!, filemodel);
            }
            return Ok(new ResponseEntity(result));

        }

        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_DEPARTMENT })]
        public async Task<IActionResult> UpdateDepartmentOrder()
        {

            var model = Request.Form["OrderObj"][0]?.StringToObject<List<OrderingDTO>>();
            var result = await masterBL.GetAdminService<SrvDepartmentBL>().UpdateDepartmentOrder(model!);
            return Ok(result);

        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_DEPARTMENT })]
        public async Task<IActionResult> DeleteDepartment(Guid Id)
        {

            var result = await masterBL.GetAdminService<SrvDepartmentBL>().DeleteDepartment(Id);
            return Ok(result);

        }

        public async Task<IActionResult> GetDepartmentClass()
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            var DepClass = await masterBL.GetAdminService<SrvDepartmentBL>().GetDepartmentClass();
            response.Add("DepartmentClass", DepClass);
            return Ok(new ResponseEntity(response));
        }

    }
}
