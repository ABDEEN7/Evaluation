using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.DAL.Models.Template;
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
    public class TemplateDocumentController : Controller
    {
        
        private readonly MasterBL masterBL;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly AzureBlobStorageService _blobService;


        public TemplateDocumentController( MasterBL masterBL, AzureBlobStorageService _blobService, IHttpContextAccessor httpContextAccessor)
        {

           
            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;
            this._blobService = _blobService;
        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_TEMPLATEDOC })]
        public async Task<IActionResult> Index()
        {
            var model = new TemplateDocumentVM(httpContextAccessor);
            await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminTemplateDocument },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_TEMPLATEDOC },
                new string[] {

                 ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT
            });
            var property = typeof(TemplateDocument).GetProperty("OrderNo");
            model.containsOrderNo = property != null ? true : false;
            return View(model);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_TEMPLATEDOC })]
        public async Task<IActionResult> GetAllTemplateDocument(Guid serviceId,int Page = 1)
        {
            var PageSize = Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
            var response = await masterBL.GetAdminService<SrvTemplateDocumentBL>().GetTemplateDocList(Page, PageSize,serviceId);
            return Ok(response);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_TEMPLATEDOC })]
        public async Task<IActionResult> GetAttachmentById(Guid Id)
        {
           
                var result = await masterBL.GetAdminService<SrvTemplateDocumentBL>().GetAttachment(Id);
                return Ok(result);
            
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_TEMPLATEDOC })]
        public async Task<IActionResult> SaveTemplateDocument()
        {
            
           
                var files = Request.Form.Files;
                var result = new TempLateDocDTO();
                var model = Request.Form["request"][0]?.StringToObject<TempLateDocDTO>();
            List<WebsiteAttachmentDTO> filemodel = new  List<WebsiteAttachmentDTO>();
            if (null != files && model?.IsAttachment == true)
                {
                    var FinalFiles = files.Where(c => c.Length > 0).ToList();
                    if (FinalFiles.Count > 0)
                    {
                        var constraintList = await masterBL.GetAdminService<SrvSystemSettingBL>().GetAppConstraints(ConstantKeys.AdminPermission.ADD_ADMIN_TEMPLATEDOC );

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

                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(model!, ConstantKeys.AdminPermission.ADD_ADMIN_TEMPLATEDOC);
                if (validateObject)
                {
                    result = await masterBL.GetAdminService<SrvTemplateDocumentBL>().SaveTemplateDoc(model!, filemodel);
                }
                return Ok(new ResponseEntity(result));
            
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_TEMPLATEDOC })]
        public async Task<IActionResult> UpdateTemplateDocument()
        {
            
                var files = Request.Form.Files;
                var result = new TempLateDocDTO();
                var model = Request.Form["request"][0]?.StringToObject<TempLateDocDTO>();
            List<WebsiteAttachmentDTO> filemodel = new  List<WebsiteAttachmentDTO>();
            if (null != files && model?.IsAttachment==true)
                {
                    var FinalFiles = files.Where(c => c.Length > 0).ToList();
                    if (FinalFiles.Count > 0)
                    {
                        var constraintList = await masterBL.GetAdminService<SrvSystemSettingBL>().GetAppConstraints(ConstantKeys.AdminPermission.ADD_ADMIN_TEMPLATEDOC);

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

                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(model!, ConstantKeys.AdminPermission.ADD_ADMIN_TEMPLATEDOC);
                if (validateObject)
                {
                    result = await masterBL.GetAdminService<SrvTemplateDocumentBL>().UpdateTemplateDoc(model!, filemodel);
                }
                return Ok(new ResponseEntity(result));
           
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_TEMPLATEDOC })]
        public async Task<IActionResult> DeleteTemplateDocument(Guid Id)
        {
            
                var result = await masterBL.GetAdminService<SrvTemplateDocumentBL>().DeleteTemplateDoc(Id);
                return Ok(result);
            
        }

    }
}
