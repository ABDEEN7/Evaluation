using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.Models.Admin;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            var PageSize =  Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
            var response = await masterBL.GetAdminService<SrvDepartmentBL>().GetDepartmentList(Page, PageSize);
            return Ok(response);
        }
       
        
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_DEPARTMENT })]
        public async Task<IActionResult> SaveDepartment()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<DepartmentDTO>();
                var result = new DepartmentDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_DEPARTMENT);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvDepartmentBL>().SaveDepartment(request!);
                    
                }
                return Ok(new ResponseEntity(result));
           
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_DEPARTMENT })]
        public async Task<IActionResult> UpdateDepartment()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<DepartmentDTO>();
                var result = new DepartmentDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_DEPARTMENT);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvDepartmentBL>().UpdateDepartment(request!);
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

    }
}
