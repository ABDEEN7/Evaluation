using Evaluation.Api.Extensions;
using Evaluation.API.Filters;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.Models.API;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Evaluation.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController(MasterBL masterBl, IHttpContextAccessor httpContextAccessor) : ControllerBase
    {
        private readonly MasterBL masterBl = masterBl;
 
        [HttpPost()]
        public async Task<WebAppConfigsResponse> GetWebAppConfigs()
        {
            var model = Request.Form?["request"].ElementAtOrDefault(0)?.StringToObject<WebAppConfigsRequest>() ?? new WebAppConfigsRequest();
            var result = await masterBl.CacheDataProvider.GetWebAppConfigs(model);

            return result;

        }


        //[HttpPost]
        //public async Task<IActionResult> GetPageControls()
        //{
        //    var pages = Request.Form["pages"].ElementAtOrDefault(0)?.StringToObject<List<string>>()?.ToArray() ?? [];
        //    var permissions = Request.Form["permissions"].ElementAtOrDefault(0)?.StringToObject<List<string>>()?.ToArray() ?? [];
        //    var systemSettings = Request.Form["systemSettings"].ElementAtOrDefault(0)?.StringToObject<List<string>>()?.ToArray() ?? [];

        //    var model = new BaseVM(httpContextAccessor);
        //    await model.LoadAllAData(pages, permissions, systemSettings);
        //    return Ok(model);
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetSearchResult(int page, string? query)
        //{

        //    var result = await masterBl.GetApiService<SearchBL>().GetSearchResult(query, page);
        //    return Ok(result);
        //}

        [HttpGet()]
        [Obsolete("Should not used")]
        public async Task<List<UiControlItemDTO>> GetControlsforPage()
        {
            var permissions = new string[] { "ADD_ADMIN_NAVBAR" };
            var Pages = new string[] { "AdminNavbar" };
            var result = await masterBl.GetApiService<UiControlBL>().LoadAllControlValidations(permissions, Pages);
            return result;
        }



        [Authorize]

        [ServiceFilter(typeof(TokenValidationFilter))]
        [HttpGet()]
        public string Test()
        {
            return "";
        }

        //#region SystemLogo-url
        //[HttpGet]
        //public IActionResult GetSystemLogo()
        //{
           
        //    var FaviconSetting = masterBl.GetApiService<UiControlBL>().GetSetting(ConstantKeys.AdminSettings.Favicon);
        //    var WebLogoSetting = masterBl.GetApiService<UiControlBL>().GetWebLogo();
        //    Dictionary<string, object> response = new Dictionary<string, object>();

        //    response.Add("FaviconSetting", FaviconSetting);
        //    response.Add("WebLogoSetting", WebLogoSetting);
        //    return Ok(response);
        //}
        //#endregion
       

        protected ClaimsIdentity GetClaimsIdentity()
        {

            if (User.Identity!.IsAuthenticated)
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                return claimsIdentity!;
            }

            throw new UnauthorizedAccessException();
        }

        //[HttpGet()]
        //[DisableCors]
        //public async Task<IActionResult> ClearCache(string name, string securitykey)
        //{
        //    if (!string.IsNullOrEmpty(securitykey))
        //    {

        //        var dbsecuritykey = await masterBl.CacheDataProvider.GetSettingfromDB(ConstantKeys.AdminSettings.SecurityKey);
        //        if (dbsecuritykey == securitykey)
        //        {
        //            string Message = "Clear Cache By Key From Web site  main controller:  key =" + name;
                   
        //            if (!string.IsNullOrEmpty(name))
        //            {
        //                await masterBl.CacheDataProvider.ClearCacheByKey(name,false);
        //                Dictionary<string, object> response = new Dictionary<string, object>();
        //                response.Add("Message", "Success");
        //                return Ok(response);
        //            }
                    
        //        }
        //    }

        //    throw new UnauthorizedAccessException();

        //}
    }
}
