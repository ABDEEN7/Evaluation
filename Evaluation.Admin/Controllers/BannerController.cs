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
    public class BannerController : Controller
    {
       
        private readonly MasterBL masterBL;
        
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly AzureBlobStorageService _blobService;
        public BannerController( MasterBL masterBL, IHttpContextAccessor httpContextAccessor, AzureBlobStorageService blobService)
        {

            
            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;
            _blobService = blobService;
            
        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_BANNER })]
        public async Task<IActionResult> Index()
        {
            var model = new BannerVM(httpContextAccessor);
            await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminBanner },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_BANNER },
                 new string[] {
                ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT
                 });

            var property = typeof(Banner).GetProperty("OrderNo");
            model.containsOrderNo = property != null ? true:false ;
            return View(model);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_BANNER })]
        public async Task<IActionResult> GetAllBanner(int Page = 1)
        {
            var PageSize =   Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));           
            var response = await masterBL.GetAdminService<SrvBannerBL>().GetBannerList(Page, PageSize);
            return Ok(response);
        }

        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_BANNER })]
        public async Task<IActionResult> SaveBanner()
        {
           
                var files = Request.Form.Files;
                var result = new BannerDTO();
                var model = Request.Form["request"][0]?.StringToObject<BannerDTO>();
                if (null != files)
                {
                    var FinalFiles = files.Where(c => c.Length > 0).ToList();
                    if(FinalFiles.Count>0)
                    {
                        var constraintList = await masterBL.GetAdminService<SrvSystemSettingBL>().GetAppConstraints(ConstantKeys.AdminPermission.ADD_ADMIN_BANNER);
                        
                       var response = await masterBL.GetAdminService<SrvBaseBL>().ValidateFiles(model!, constraintList, FinalFiles);
                        if(response!=null)
                        {
                            if (response.ResponseStatus == true)
                            {
                                model = (BannerDTO)response.Data!;

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

                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(model!, ConstantKeys.AdminPermission.ADD_ADMIN_BANNER);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvBannerBL>().SaveBanner(model!);
                }
                return Ok(result);
            
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_BANNER })]
        public async Task<IActionResult> UpdateBanner()
        {
           
                var files = Request.Form.Files;
                var result = new BannerDTO();
                var model = Request.Form["request"][0]?.StringToObject<BannerDTO>();
                if (null != files)
                {
                    var FinalFiles = files.Where(c => c.Length > 0).ToList();
                    if (FinalFiles.Count > 0)
                    {
                        var constraintList = await masterBL.GetAdminService<SrvSystemSettingBL>().GetAppConstraints(ConstantKeys.AdminPermission.ADD_ADMIN_BANNER);

                        var response = await masterBL.GetAdminService<SrvBaseBL>().ValidateFiles(model!, constraintList, FinalFiles);
                        if (response != null)
                        {
                            if (response.ResponseStatus == true)
                            {
                                model = (BannerDTO)response.Data!;

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
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(model!, ConstantKeys.AdminPermission.ADD_ADMIN_BANNER);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvBannerBL>().UpdateBanner(model!);
                }
            return Ok(result);

        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_BANNER })]
        public async Task<IActionResult> UpdateBannerOrder()
        {
           
                var model = Request.Form["OrderObj"][0]?.StringToObject<List<OrderingDTO>>();
                var result = await masterBL.GetAdminService<SrvBannerBL>().UpdateBannerOrder(model!);
                return Ok(result);
           
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_BANNER })]
        public async Task<IActionResult> DeleteBanner(Guid Id)
        {
           
                var result = await masterBL.GetAdminService<SrvBannerBL>().DeleteBanner(Id);
                return Ok(result);
           
        }

    }
}
