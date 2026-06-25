using Evaluation.DAL.Dtos;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API;
using Evaluation.Services.BusinessLayer.API.DepartmentLayer;
using Evaluation.Services.BusinessLayer.API.WebGroupLayer;
using Evaluation.SharedHelper.Dtos.WebSiteDto;
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
        public IActionResult GetDepartment([FromQuery] string routingPath)
        {
            var lang = _requestInfo.Lang;
            return Ok(new { result = GetDepartmentByRoutingPath(routingPath, lang) });
        }

        [HttpGet]
        public IActionResult GetWebGroup([FromQuery] string webGroupPath)
        {
            return Ok(new { result = GetWebGroupByWebGroupPath(webGroupPath) });
        }

        [HttpGet]
        public IActionResult GetDepartmentsForWebGroup([FromQuery] string webGroupPath)
        {
            return Ok(new { result = GetAllDepartmentsForWebGroupList(webGroupPath) });
        }
        [HttpGet()]
        public async Task<List<NavbarDTO>> GetNavbar(string pathParts)
        {
            var lang = _requestInfo.Lang;
            var navbars = await _masterBl.GetApiService<WebsiteBL>().GetNavbarList(pathParts, lang);
            return navbars;
        }
        [HttpGet()]
        public async Task<List<BannerDTO>> GetBanner([FromQuery] string webGroupPath)
        {
            var lang = _requestInfo.Lang;

            var banners = await _masterBl.GetApiService<WebsiteBL>().GetBanners(webGroupPath, lang);
            return banners;
        }


        private async Task<List<DepartmentDto>> GetAllDepartmentsForWebGroupList(string webGroupPath)
        {
            return await _masterBl.GetApiService<DepartmentBL>().GetAllDepartmentsForWebGroup(webGroupPath);
        }

        private async Task<List<DepartmentDto>> GetDepartmentList()
        {
            return await _masterBl.GetApiService<DepartmentBL>().GetAllDepartments();
        }

        private async Task<DepartmentDto> GetDepartmentByRoutingPath(string routingPath, string lang)
        {
            return await _masterBl.GetApiService<DepartmentBL>().GetDepartmentByRoutingPath(routingPath, lang);
        }
        private async Task<WebGroupsDto> GetWebGroupByWebGroupPath(string webGroupPath)
        {
            return await _masterBl.GetApiService<WebGroupBL>().GetWebGroupByPath(webGroupPath);
        }
    }
}
