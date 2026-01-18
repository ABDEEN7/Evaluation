using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API.DepartmentHolidayLayer;
using Evaluation.SharedHelper.Models.Api.DepartmentHolidaysDto;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class DepartmentHolidayController : ControllerBase
{
    private readonly MasterBL _masterBL;

    public DepartmentHolidayController(MasterBL masterBL)
    {
        _masterBL = masterBL;
    }
    [HttpGet]
    public async Task<IActionResult> GetAllHolidayDepartments(int page = 1)
    {
        return Ok(await _masterBL.GetApiService<DepartmentHolidayBL>().GetDepartmentHolidayList(page));
    }
    [HttpPost]
    public async Task<IActionResult> AddDepartmentHoliday(CreateDepartmentHolidayDto model)
    {
        return Ok(await _masterBL.GetApiService<DepartmentHolidayBL>().AddDepartmentHoliday(model));
    }
    [HttpPost]
    public async Task<IActionResult> UpdateDepartmentHoliday(UpdateDepartmentHolidayDto model)
    {
        return Ok(await _masterBL.GetApiService<DepartmentHolidayBL>().UpdateDepartmentHoliday(model));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteDepartmentHoliday(Guid departmentHolidayId)
    {
        return Ok(await _masterBL.GetApiService<DepartmentHolidayBL>().DeleteDepartmentHoliday(departmentHolidayId));
    }
}
