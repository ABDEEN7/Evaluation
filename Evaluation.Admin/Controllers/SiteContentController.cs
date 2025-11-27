using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.Models.Admin;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Admin.Controllers
{
    [Authorize]
    public class SiteContentController : Controller
    {
      
        private readonly MasterBL masterBL;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly AzureBlobStorageService _blobService;
        public SiteContentController( MasterBL masterBL, AzureBlobStorageService _blobService, IHttpContextAccessor httpContextAccessor)
        {

           
            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;
            this._blobService = _blobService;
        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_SITECONTENT })]
        public async Task<IActionResult> Index()
        {
            var model = new SiteContentVM(httpContextAccessor);
            await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminSiteContent },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_SITECONTENT },
                new string[] {

                 ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT
            });
            return View(model);
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_SITECONTENT })]

        public async Task<IActionResult> GetAllSiteContent([FromBody] AdminSearchDTO request)
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
            var response = await masterBL.GetAdminService<SrvSiteContentBL>().GetSiteContentList(request);
            return Ok(response);
        }

        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_SITECONTENT })]
        public async Task<IActionResult> GetRawSiteContentList()
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            var Parent = await masterBL.GetAdminService<SrvSiteContentBL>().GetRawSiteContentList();
            response.Add("Parent", Parent);
            return Ok(new ResponseEntity(response));
        }

        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_SITECONTENT })]
        public async Task<IActionResult> SaveSiteContent()
        {
            
                var files = Request.Form.Files;
                var result = new SiteContentDTO();
                var model = Request.Form["request"][0]?.StringToObject<SiteContentDTO>();
                if (null != files)
                {
                    var FinalFiles = files.Where(c => c.Length > 0).ToList();
                    if (FinalFiles.Count > 0)
                    {
                        var constraintList = await masterBL.GetAdminService<SrvSystemSettingBL>().GetAppConstraints(ConstantKeys.AdminPermission.ADD_ADMIN_SITECONTENT);

                        var response = await masterBL.GetAdminService<SrvBaseBL>().ValidateFiles(model!, constraintList, FinalFiles);
                        if (response != null)
                        {
                            if (response.ResponseStatus == true)
                            {
                                model = (SiteContentDTO)response.Data!;

                            }
                            else
                            {
                                model!.ResponseMessage = response.ResponseMessage;

                                model.ResponseState = response.ResponseStatus;

                                return Json(new ResponseEntity(model));
                            }
                        }

                    }


                }

                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(model!, ConstantKeys.AdminPermission.ADD_ADMIN_SITECONTENT);
                if (validateObject)
                {
                    result = await masterBL.GetAdminService<SrvSiteContentBL>().SaveSiteContent(model!);
                }
                return Ok(new ResponseEntity(result));
            
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_SITECONTENT })]
        public async Task<IActionResult> UpdateSiteContent()
        {
            
                var files = Request.Form.Files;
                var result = new SiteContentDTO();
                var model = Request.Form["request"][0]?.StringToObject<SiteContentDTO>();
                if (null != files)
                {
                    var FinalFiles = files.Where(c => c.Length > 0).ToList();
                    if (FinalFiles.Count > 0)
                    {
                        var constraintList = await masterBL.GetAdminService<SrvSystemSettingBL>().GetAppConstraints(ConstantKeys.AdminPermission.ADD_ADMIN_SITECONTENT);

                        var response = await masterBL.GetAdminService<SrvBaseBL>().ValidateFiles(model!, constraintList, FinalFiles);
                        if (response != null)
                        {
                            if (response.ResponseStatus == true)
                            {
                                model = (SiteContentDTO)response.Data!;

                            }
                            else
                            {
                                model!.ResponseMessage = response.ResponseMessage;

                                model.ResponseState = response.ResponseStatus;

                                return Json(new ResponseEntity(model));
                            }
                        }

                    }


                }
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(model!, ConstantKeys.AdminPermission.ADD_ADMIN_SITECONTENT);
                if (validateObject)
                {
                    result = await masterBL.GetAdminService<SrvSiteContentBL>().UpdateSiteContent(model!);
                }
                return Ok(new ResponseEntity(result));
           
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_SITECONTENT })]
        public async Task<IActionResult> UpdateSiteContentOrder()
        {
           
                var model = Request.Form["OrderObj"][0]?.StringToObject<List<OrderingDTO>>();
                var result = await masterBL.GetAdminService<SrvSiteContentBL>().UpdateSiteContentOrder(model!);
                return Ok(result);
            
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_SITECONTENT })]
        public async Task<IActionResult> DeleteSiteContent(Guid Id)
        {
           
                var result = await masterBL.GetAdminService<SrvSiteContentBL>().DeleteSiteContent(Id);
                return Ok(result);
            
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_SITECONTENT })]
        public async Task<IActionResult> GetNavbar()
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            var Navbar = await masterBL.GetAdminService<SrvSiteContentBL>().GetNavbarList();
            response.Add("Navbar", Navbar);
            return Ok(new ResponseEntity(response));
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_SITECONTENT })]
        public async Task<IActionResult> GetNavbarListFromSiteContent()
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            var Navbar = await masterBL.GetAdminService<SrvSiteContentBL>().GetNavbarListFromSiteContent();
            response.Add("Navbar", Navbar);
            return Ok(new ResponseEntity(response));
        }

    }
}
