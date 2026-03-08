using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.ServiceEnities;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.Models.Admin;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Admin.Controllers
{
    [Authorize]
    public class ServiceController : Controller
    {

        private readonly UserInfo userInfoSession;
        private readonly MasterBL masterBL;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ServiceController(UserInfo userInfoSession, MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
        {

            this.userInfoSession = userInfoSession;
            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;

        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_SERVICE })]
        public async Task<IActionResult> Index()
        {
            var model = new ServiceVM(httpContextAccessor);
            await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminService,
            ConstantKeys.AdminPages.AdminPlaceHolder,
            ConstantKeys.AdminPages.AdminServiceFreeze},
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_SERVICE,
                ConstantKeys.AdminPermission.ADD_ADMIN_SERVICE_PLACEHOLDER,
                ConstantKeys.AdminPermission.FREEZE_ADMIN_SERVICE},
                new string[] {
                 ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT
            });

            var property = typeof(Service).GetProperty("OrderNo");
            model.containsOrderNo = property != null ? true : false;
            return View(model);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_SERVICE })]
        public async Task<IActionResult> GetAllService(SearchServiceDto searchService, int Page = 1)
        {
            var PageSize = Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
            var response = await masterBL.GetAdminService<SrvServiceBL>().GetServiceList(Page, PageSize, searchService);
            return Ok(response);
        }


        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_SERVICE })]
        public async Task<IActionResult> SaveService()
        {

            var request = Request.Form["request"][0]?.StringToObject<ServiceDTO>();

            var result = new ServiceDTO();
            bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_SERVICE);
            if (validateObject)
            {
                result = await masterBL.GetAdminService<SrvServiceBL>().SaveService(request!);

            }
            return Ok(new ResponseEntity(result));

        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_SERVICE })]
        public async Task<IActionResult> UpdateService()
        {

            var request = Request.Form["request"][0]?.StringToObject<ServiceDTO>();

            var result = new ServiceDTO();
            bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_SERVICE);
            if (validateObject)
            {
                result = await masterBL.GetAdminService<SrvServiceBL>().UpdateService(request!);
            }
            return Ok(new ResponseEntity(result));

        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_SERVICE })]
        public async Task<IActionResult> UpdateServiceOrder()
        {

            var model = Request.Form["OrderObj"][0]?.StringToObject<List<OrderingDTO>>();
            var result = await masterBL.GetAdminService<SrvServiceBL>().UpdateServiceOrder(model!);
            return Ok(result);

        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_SERVICE })]
        public async Task<IActionResult> UpdateServiceIsFreeze()
        {

            var model = Request.Form["request"][0]?.StringToObject<ServiceDTO>();
            var result = await masterBL.GetAdminService<SrvServiceBL>().UpdateServiceIsFreeze(model!);
            return Ok(result);

        }

        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_SERVICE })]
        public async Task<IActionResult> DeleteService(Guid Id)
        {

            var result = await masterBL.GetAdminService<SrvServiceBL>().DeleteService(Id);
            return Ok(result);

        }

        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_SERVICE_PLACEHOLDER })]
        public async Task<IActionResult> GetAllFields(Guid serviceid, Guid systemmoduleid)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            var RequestField = await masterBL.GetAdminService<SrvServiceBL>().GetRequestField(serviceid);
            var EvaluationField = await masterBL.GetAdminService<SrvServiceBL>().GetEvaluationField(systemmoduleid);
            response.Add("RequestField", RequestField);
            response.Add("EvaluationField", EvaluationField);
            return Ok(response);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_SERVICE_PLACEHOLDER })]
        public async Task<IActionResult> GetAllChildFields(Guid FieldId)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            var ChildFields = await masterBL.GetAdminService<SrvServiceBL>().GetAllChildFields(FieldId);
            response.Add("ChildFields", ChildFields);
            return Ok(response);
        }

        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_SERVICE_PLACEHOLDER })]
        public async Task<IActionResult> GetAllPlaceHolder(Guid serviceid)
        {
            var PageSize = masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE).FirstOrDefault();
            var response = await masterBL.GetAdminService<SrvServiceBL>().GetPlaceHolderList(serviceid);
            return Ok(response);
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_SERVICE_PLACEHOLDER })]
        public async Task<IActionResult> SavePlaceholder()
        {

            var request = Request.Form["request"][0]?.StringToObject<PlaceHolderDTO>();
            var result = new PlaceHolderDTO();
            bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_SERVICE_PLACEHOLDER);
            if (validateObject)
            {
                result = await masterBL.GetAdminService<SrvServiceBL>().SavePlaceHolder(request!);

            }
            return Ok(new ResponseEntity(result));

        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_SERVICE_PLACEHOLDER })]
        public async Task<IActionResult> UpdatePlaceholder()
        {

            var request = Request.Form["request"][0]?.StringToObject<PlaceHolderDTO>();
            var result = new PlaceHolderDTO();
            bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_SERVICE_PLACEHOLDER);
            if (validateObject)
            {
                result = await masterBL.GetAdminService<SrvServiceBL>().UpdatePlaceHolder(request!);
            }
            return Ok(new ResponseEntity(result));

        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_SERVICE_PLACEHOLDER })]
        public async Task<IActionResult> DeletePlaceHolder(Guid Id)
        {

            var result = await masterBL.GetAdminService<SrvServiceBL>().DeletePlaceHolder(Id);
            return Ok(result);

        }

    }
}
