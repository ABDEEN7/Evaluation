using Evaluation.API.ActionFilter;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API.DepartmentHolidayLayer;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models.Api.DepartmentHolidaysDto;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.API.Controllers;

[ApiController]
[Route("api/[controller]/{depRouting}/[action]")]
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
    [HttpGet]
    [CheckRolePermisionFilter(true, ConstantKeys.WebPermissions.VIEW_WEB_DepartmentHoliday)]
    public async Task<IActionResult> GetAllHolidayDepartmentsOrg()
    {
        return Ok(await _masterBL.GetApiService<DepartmentHolidayBL>().GetDepartmentHolidayList());
    }
    [HttpPost]
    [CheckRolePermisionFilter(true, ConstantKeys.WebPermissions.ADD_WEB_DEPARTMENT_HOLIDAY)]
    public async Task<IActionResult> AddDepartmentHoliday(CreateDepartmentHolidayDto model)
    {
        return Ok(await _masterBL.GetApiService<DepartmentHolidayBL>().AddDepartmentHoliday(model));
    }
    [HttpPost]
    [CheckRolePermisionFilter(true, ConstantKeys.WebPermissions.EDIT_WEB_DEPARTMENT_HOLIDAY)]
    public async Task<IActionResult> UpdateDepartmentHoliday(UpdateDepartmentHolidayDto model)
    {
        return Ok(await _masterBL.GetApiService<DepartmentHolidayBL>().UpdateDepartmentHoliday(model));
    }

    [HttpPost]
    [CheckRolePermisionFilter(true, ConstantKeys.WebPermissions.DELETE_WEB_DEPARTMENT_HOLIDAY)]
    public async Task<IActionResult> DeleteDepartmentHoliday(Guid departmentHolidayId)
    {
        return Ok(await _masterBL.GetApiService<DepartmentHolidayBL>().DeleteDepartmentHoliday(departmentHolidayId));
    }
}
