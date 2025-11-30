using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Website;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.Models.Admin;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Evaluation.Admin.Controllers
{
    [Authorize]
    public class NavbarController : Controller
    {
        
        private readonly UserInfo userInfoSession;
        private readonly MasterBL masterBL;
        private readonly IHttpContextAccessor httpContextAccessor;

        public NavbarController(UserInfo userInfoSession, MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
        {

            this.userInfoSession = userInfoSession;
            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;
           
        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_NAVBAR })]
        public async Task<IActionResult> Index()
        {
            var model = new NavbarVM(httpContextAccessor);
            await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminNavbar },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_NAVBAR },
                new string[] {
                 ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT
            });

            var property = typeof(Navbar).GetProperty("OrderNo");
            model.containsOrderNo = property != null ? true : false;
            return View(model);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_NAVBAR })]
        public async Task<IActionResult> GetAllNavbar(Guid id,int Page = 1)
        {
            var PageSize =  Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
            var response = await masterBL.GetAdminService<SrvNavbarBL>().GetNavbarList(id,Page, PageSize);
            return Ok(response);
        }
       
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_NAVBAR })]
        public async Task<IActionResult> GetAllParentNavbar()
        {

            Dictionary<string, object> response = new Dictionary<string, object>();
            var Navbar = await masterBL.GetAdminService<SrvNavbarBL>().GetParentNavbarListWithMater();
            response.Add("Navbar", Navbar);
            return Ok(new ResponseEntity(response));
        }
        
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_NAVBAR })]
        public async Task<IActionResult> GetNavbarListWithUpAndDownLevel()
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            var Navbar = await masterBL.GetAdminService<SrvNavbarBL>().GetNavbarListWithUpAndDownLevel();
           // Navbar = Navbar.Where(x => x.Level < 1).ToList();
            response.Add("Navbar", Navbar);
            return Ok(new ResponseEntity(response));
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_NAVBAR })]
        public async Task<IActionResult> GetNavbarPermissionList()
        {
            string[] settigKeys = new string[] {
                ConstantKeys.AdminSettings.NavBarPermissionList

            };
            Dictionary<string, object> response = new Dictionary<string, object>();
            var SettingTask = await masterBL.GetAdminService<SrvSystemSettingBL>().GetSettings(settigKeys.ToList());
            var Settintaskvalue = SettingTask.Select(x => x.SettingValue).ToList();
            if(Settintaskvalue[0]!=null)
            {
                List<DropdownItem> result = new List<DropdownItem>();
                var jsonArray = JArray.Parse(Settintaskvalue[0]!);
                foreach (var data in jsonArray)
                {
                    DropdownItem item = new DropdownItem();
                    item.Id = (string)data["Id"]!;
                    item.NameAr = (string)data["TitleAr"]!;
                    item.NameEn = (string)data["TitleEn"]!;
                    result.Add(item);
                }
                response.Add("NavBarPermission", result);
            }
            
            
            return Ok(new ResponseEntity(response));
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_NAVBAR })]
        public async Task<IActionResult> GetRoutingList()
        {
           
            Dictionary<string, object> response = new Dictionary<string, object>();
            var Routing = await masterBL.GetAdminService<SrvNavbarBL>().GetRoutingList();
            var Routinglist = await GetSettingRoutingList();
            var AllRoutinglist = Routing.Union(Routinglist);
            response.Add("Routing", AllRoutinglist);
            return Ok(new ResponseEntity(response));
        }
        public async Task<List<SiteContentDTO>> GetSettingRoutingList()
        {
            string[] settigKeys = new string[] {
                ConstantKeys.AdminSettings.RoutingValue

            };
            Dictionary<string, object> response = new Dictionary<string, object>();
            var SettingTask = await masterBL.GetAdminService<SrvSystemSettingBL>().GetSettings(settigKeys.ToList());
            var Settintaskvalue = SettingTask.Select(x => x.SettingValue).ToList();
            List<SiteContentDTO> rslt = new List<SiteContentDTO>();
            if(Settintaskvalue[0] != null)
            {
                var jsonArray = JArray.Parse(Settintaskvalue[0]!);
                foreach (var data in jsonArray)
                {
                    SiteContentDTO rslt1 = new SiteContentDTO();
                    rslt1.Routing = (string)data["Routing"]!;
                    rslt1.TitleAr = (string)data["TitleAr"]!;
                    rslt1.TitleEn = (string)data["TitleEn"]!;
                    rslt.Add(rslt1);
                }
            }
           


            return rslt;
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_NAVBAR })]
        public async Task<IActionResult> SaveNavbar()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<NavbarDTO>();
                var result = new NavbarDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_NAVBAR);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvNavbarBL>().SaveNavbar(request!);
                    
                }
                return Ok(new ResponseEntity(result));
           
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_NAVBAR })]
        public async Task<IActionResult> UpdateNavbar()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<NavbarDTO>();
                var result = new NavbarDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_NAVBAR);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvNavbarBL>().UpdateNavbar(request!);
                }
                return Ok(new ResponseEntity(result));
            
        }

        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_NAVBAR })]
        public async Task<IActionResult> UpdateNavbarOrder()
        {
           
                var model = Request.Form["OrderObj"][0]?.StringToObject<List<OrderingDTO>>();
                var result = await masterBL.GetAdminService<SrvNavbarBL>().UpdateNavbarOrder(model!);
                return Ok(result);
            
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_NAVBAR })]
        public async Task<IActionResult> DeleteNavbar(Guid Id)
        {
           
                var result = await masterBL.GetAdminService<SrvNavbarBL>().DeleteNavbar(Id);
                return Ok(result);
           
        }

    }
}
