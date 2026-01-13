using Evaluation.DAL.Dtos;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API;
using Evaluation.Services.BusinessLayer.API.DepartmentLayer;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api.WebsiteDTOs;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WebsiteController : Controller
    {
        private readonly MasterBL _masterBl;
        private readonly RequestInfo _requestInfo;
        public WebsiteController(MasterBL masterBl, RequestInfo requestInfo)
        {
            this._masterBl = masterBl;
            _requestInfo = requestInfo;
        }

        [HttpGet]
        public IActionResult GetDepartments()
        {
            return Ok(new { result = GetDepartmentList() });
        }

        [HttpGet]
        public IActionResult GetDepartmentsForWebGroup([FromQuery] string webGroupPath)
        {
            return Ok(new { result = GetAllDepartmentsForWebGroupList(webGroupPath) });
        }

        private async Task<List<DepartmentDto>> GetAllDepartmentsForWebGroupList(string webGroupPath)
        {
           return await _masterBl.GetApiService<DepartmentBL>().GetAllDepartmentsForWebGroup(webGroupPath);
        }

        private async Task<List<DepartmentDto>> GetDepartmentList()
        {
           return await _masterBl.GetApiService<DepartmentBL>().GetAllDepartments();
        }
        [HttpGet()]
        public async Task<List<NavbarDTO>> GetNavbar()
        {
            var lang = _requestInfo.Lang;
            var navbars = await _masterBl.GetApiService<WebsiteBL>().GetNavbarList(lang);
            return navbars;
        }
        [HttpGet()]
        public async Task<List<BannerDTO>> GetBanner([FromQuery] string webGroupPath )
        {
            var lang = _requestInfo.Lang;

            var banners = await _masterBl.GetApiService<WebsiteBL>().GetBanners(webGroupPath, lang);
            return banners;
        }
    }
}
