using Evaluation.DAL.DTOs;
using Evaluation.DAL.Models.Org;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API;
using Evaluation.Services.Integration;
using Evaluation.SharedHelper.Dtos.SchoolDto;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class SchoolController : ControllerBase
{

    private readonly MasterBL _masterBl;
    private readonly HRService _hrService;
    public SchoolController(MasterBL masterBl, HRService hrService)
    {
        this._masterBl = masterBl;
        this._hrService = hrService;
    }

    [HttpGet]
    public async Task<School> GetSchoolDetails(Guid SchoolID)
    {
        var schooldetails = await _masterBl.GetApiService<SchoolBL>().GetSchoolDetails(SchoolID);
        return schooldetails;
    }

    [HttpGet]
    public async Task<List<HREmployeeInfoDto>> GetHREmployeesDetails(int page)
    {
        return await _hrService.GetAllHRUsersAsync(page);
    }

    [HttpGet]
    public async Task<List<HROrganizationInfoDto>> GetHROrgDetailsAsync(int page)
    {
        return await _hrService.GetAllHROrgAsync(page);
    }

    [HttpGet]
    public async Task<IActionResult> GetVisits()
        => Ok(new { result = await _masterBl.GetApiService<SchoolBL>().GetVisitsAsync() });

    [HttpGet]
    public async Task<IActionResult> GetSchools([FromQuery] SchoolRequest request)
    {
        var schooldetails = await _masterBl.GetApiService<SchoolBL>().GetSchools();
        return Ok(new { result = schooldetails });
    }
}