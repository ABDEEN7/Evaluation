using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.DAL.Helper;
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
    public class EmployeesController : Controller
    {
        
        private readonly UserInfo userInfoSession;
        private readonly MasterBL masterBL;
        private readonly IHttpContextAccessor httpContextAccessor;

        public EmployeesController(UserInfo userInfoSession, MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
        {

            this.userInfoSession = userInfoSession;
            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;
           
        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_EMPLOYEE })]
        public async Task<IActionResult> Index()
        {
            var model = new EmployeesVM(httpContextAccessor);
            await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminEmployees },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_EMPLOYEE },
                new string[] {
                 ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT
            });
           
            var property = typeof(Employee).GetProperty("OrderNo");
            model.containsOrderNo = property != null ? true : false;
            return View(model);
        }

        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_EMPLOYEE })]
        public async Task<IActionResult> GetOrgClass()
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            var OrgClass = await masterBL.GetAdminService<SrvEmployeesBL>().GetOrgClass();
            response.Add("OrgClass", OrgClass);
            return Ok(new ResponseEntity(response));
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_EMPLOYEE })]
       
        public async Task<IActionResult> GetAllEmployees([FromBody] AdminSearchDTO request)
        {

            if (null == request)
            {
                request = new AdminSearchDTO
                {
                    PageNum = 0,
                    PageSize = Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE)),
                };
            }
            else
            {
                request.PageNum = request.PageNum ?? 0;
                request.PageSize = request.PageSize ?? Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
            }
            request.PageNum = request.PageNum ?? 0;
            request.PageSize = request.PageSize ?? Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
            var response = await masterBL.GetAdminService<SrvEmployeesBL>().GetEmployeesList(request);
            return Ok(response);
        }


        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_EMPLOYEE })]
        public async Task<IActionResult> SaveEmployees()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<EmployeesDTO>();

            var result = new EmployeesDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_EMPLOYEE);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvEmployeesBL>().SaveEmployees(request!);
                    
                }
                return Ok(new ResponseEntity(result));
           
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_EMPLOYEE })]
        public async Task<IActionResult> UpdateEmployees()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<EmployeesDTO>();
          
            var result = new EmployeesDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_EMPLOYEE);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvEmployeesBL>().UpdateEmployees(request!);
                }
                return Ok(new ResponseEntity(result));
            
        }

       
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_EMPLOYEE })]
        public async Task<IActionResult> DeleteEmployees(Guid Id)
        {
           
                var result = await masterBL.GetAdminService<SrvEmployeesBL>().DeleteEmployees(Id);
                return Ok(result);
           
        }

    }
}
