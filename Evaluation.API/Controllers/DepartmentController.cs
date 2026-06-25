using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API.DepartmentHolidayLayer;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.API.Controllers;

public class DepartmentController : ControllerBase
{
    private readonly MasterBL _masterBL;

    public DepartmentController(MasterBL masterBL)
    {
        _masterBL = masterBL;
    }
    [HttpGet]
    public async Task<IActionResult> GetAllHolidayDepartments(int page = 1)
    {
        return Ok(await _masterBL.GetApiService<DepartmentHolidayBL>().GetDepartmentHolidayList(page));
    }
}
