using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.DAL.Models.Website;
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
    public class SiteDocumentController : Controller
    {
        
        private readonly MasterBL masterBL;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly AzureBlobStorageService _blobService;


        public SiteDocumentController( MasterBL masterBL, AzureBlobStorageService _blobService, IHttpContextAccessor httpContextAccessor)
        {

           
            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;
            this._blobService = _blobService;
        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_SITEDOCUMENT })]
        public async Task<IActionResult> Index()
        {
            var model = new SiteDocumentVM(httpContextAccessor);
            await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminSiteDocument },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_SITEDOCUMENT },
                new string[] {

                 ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT,
                 ConstantKeys.AdminSettings.WebAppSitePath
            });
            var property = typeof(SiteDocument).GetProperty("OrderNo");
            model.containsOrderNo = property != null ? true : false;
            return View(model);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_SITEDOCUMENT })]
        public async Task<IActionResult> GetAllSiteDocument(int Page = 1)
        {
            var PageSize =  Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
            var response = await masterBL.GetAdminService<SrvSiteDocumentBL>().GetSiteDocumentList(Page, PageSize);
            return Ok(response);
        }

        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_SITEDOCUMENT })]
        public async Task<IActionResult> SaveSiteDocument()
        {
            
                var files = Request.Form.Files;
                var result = new SiteDocumentDTO();
                var model = Request.Form["request"][0]?.StringToObject<SiteDocumentDTO>();
                if (null != files)
                {
                    var FinalFiles = files.Where(c => c.Length > 0).ToList();
                    if (FinalFiles.Count > 0)
                    {
                        var constraintList = await masterBL.GetAdminService<SrvSystemSettingBL>().GetAppConstraints(ConstantKeys.AdminPermission.ADD_ADMIN_SITEDOCUMENT);

                        var response = await masterBL.GetAdminService<SrvBaseBL>().ValidateFiles(model!, constraintList, FinalFiles);
                        if (response != null)
                        {
                            if (response.ResponseStatus == true)
                            {
                                model = (SiteDocumentDTO)response.Data!;

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

                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(model!, ConstantKeys.AdminPermission.ADD_ADMIN_SITEDOCUMENT);
                if (validateObject)
                {
                    result = await masterBL.GetAdminService<SrvSiteDocumentBL>().SaveSiteDocument(model!);
                }
                return Ok(new ResponseEntity(result));
            
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_SITEDOCUMENT })]
        public async Task<IActionResult> UpdateSiteDocument()
        {
           
                var files = Request.Form.Files;
                var result = new SiteDocumentDTO();
                var model = Request.Form["request"][0]?.StringToObject<SiteDocumentDTO>();
                if (null != files)
                {
                    var FinalFiles = files.Where(c => c.Length > 0).ToList();
                    if (FinalFiles.Count > 0)
                    {
                        var constraintList = await masterBL.GetAdminService<SrvSystemSettingBL>().GetAppConstraints(ConstantKeys.AdminPermission.ADD_ADMIN_SITEDOCUMENT);

                        var response = await masterBL.GetAdminService<SrvBaseBL>().ValidateFiles(model!, constraintList, FinalFiles);
                        if (response != null)
                        {
                            if (response.ResponseStatus == true)
                            {
                                model = (SiteDocumentDTO)response.Data!;

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

                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(model!, ConstantKeys.AdminPermission.ADD_ADMIN_SITEDOCUMENT);
                if (validateObject)
                {
                    result = await masterBL.GetAdminService<SrvSiteDocumentBL>().UpdateSiteDocument(model!);
                }
                return Ok(new ResponseEntity(result));
           
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_SITEDOCUMENT })]
        public async Task<IActionResult> DeleteSiteDocument(Guid Id)
        {
           
                var result = await masterBL.GetAdminService<SrvSiteDocumentBL>().DeleteSiteDocument(Id);
                return Ok(result);
            
        }

    }
}
