using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.DAL.Entities.FormBuilder;
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
    public class DropDownController : Controller
    {
        private readonly MasterBL masterBL;
        private readonly RequestInfo _requestInfo;
        private readonly IHttpContextAccessor httpContextAccessor;
        public DropDownController(MasterBL masterBL, RequestInfo requestInfo, IHttpContextAccessor httpContextAccessor)
        {
            this.masterBL = masterBL;
            _requestInfo = requestInfo;
            this.httpContextAccessor = httpContextAccessor;
        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_DROPDOWN })]
        public async Task<IActionResult> Index()
        {
            var model = new DropDownVM(httpContextAccessor);
            await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminDropDown },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_DROPDOWN },
                new string[] {
                 ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT
            });

            var property = typeof(FieldDropDownValue).GetProperty("OrderNo");
            model.containsOrderNo = property != null ? true : false;
            return View(model);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_DROPDOWN })]
        public async Task<IActionResult> GetAllDropDown(Guid dropdowntype, int Page = 1)
        {
            var PageSize =  Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
            var response = await masterBL.GetAdminService<SrvDrodownItem>().GetDropDownList(dropdowntype,Page, PageSize);
            return Ok(response);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_DROPDOWN })]
        public async Task<IActionResult> GetAllDropDownType()
        {

            Dictionary<string, object> response = new Dictionary<string, object>();
            var DropDownType = await masterBL.GetAdminService<SrvDrodownItem>().GetDropDownTypeList();
            response.Add("DropDownType", DropDownType);
            return Ok(new ResponseEntity(response));
        }

        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_DROPDOWN })]
        public async Task<IActionResult> GetDropDownByType(Guid dropdowntype)
        {

            Dictionary<string, object> response = new Dictionary<string, object>();
            var DropDown = await masterBL.GetAdminService<SrvDrodownItem>().GetDropDownByTypeList(dropdowntype);
            response.Add("DropDown", DropDown);
            return Ok(new ResponseEntity(response));
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_DROPDOWN })]
        public async Task<IActionResult> SaveDropDown()
        {

            var request = Request.Form["request"][0]?.StringToObject<FieldDropDownValueDTO>();
            var result = new FieldDropDownValueDTO();
            bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_DROPDOWN);
            if (validateObject)
            {
                result = await masterBL.GetAdminService<SrvDrodownItem>().SaveDropDown(request!);

            }
            return Ok(result);

        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_DROPDOWN })]
        public async Task<IActionResult> UpdateDropDown()
        {

            var request = Request.Form["request"][0]?.StringToObject<FieldDropDownValueDTO>();
            var result = new FieldDropDownValueDTO();
            bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_DROPDOWN);
            if (validateObject)
            {
                result = await masterBL.GetAdminService<SrvDrodownItem>().UpdateDropDown(request!);
            }
            return Ok(result);

        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_DROPDOWN })]
        public async Task<IActionResult> UpdateDropDownOrder()
        {

            var model = Request.Form["OrderObj"][0]?.StringToObject<List<OrderingDTO>>();
            var result = await masterBL.GetAdminService<SrvDrodownItem>().UpdateDropDownOrder(model!);
            return Ok(result);

        }

        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_DROPDOWN })]
        public async Task<IActionResult> DeleteDropDown(Guid Id)
        {

            var result = await masterBL.GetAdminService<SrvDrodownItem>().DeleteDropDown(Id);
            return Ok(result);

        }
        [HttpPost()]
        public async Task<List<DropdownItem>> GetDropDownValues([FromBody] DropDownValuesRequestDTO model)
        {
            var result = new List<DropdownItem>();

            result = await masterBL.GetAdminService<SrvDrodownItem>().GetDropDownValues(model);
            return result;
        }

    }
}
