using Evaluation.DAL.Dtos;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API.DepartmentLayer;
using Evaluation.Services.Integration;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WebsiteController : Controller
    {
        private readonly MasterBL _masterBl;
        public WebsiteController(MasterBL masterBl)
        {
            this._masterBl = masterBl;
        }

        [HttpGet]
        public IActionResult GetDepartments()
        {
            return Ok(new { result = GetDepartmentList() });
        }

        private async Task<List<DepartmentDto>> GetDepartmentList()
        {
           return await _masterBl.GetApiService<DepartmentBL>().GetAllDepartments();
        }
    }
}
