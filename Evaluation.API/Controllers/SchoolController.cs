using Evaluation.API.ActionFilter;
using Evaluation.DAL.DTOs;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API;
using Evaluation.Services.Integration;
using Evaluation.SharedHelper.Dtos.SchoolDto;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models.Api.ServiceRequestEntitiesDTO;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.API.Controllers;

[ApiController]
[Route("api/[controller]/{depRouting}/[action]")]

public class SchoolController : ControllerBase
{
    private readonly MasterBL _masterBl;
    public SchoolController(MasterBL masterBl)
    {
        this._masterBl = masterBl;
    }

    [HttpGet]
    //[CheckRolePermisionFilter(true, ConstantKeys.WebPermissions.VIEW_WEB_SCHOOL)]
    public async Task<ResponseSchools> GetSchoolDetails(Guid SchoolID)
    {
        var schooldetails = await _masterBl.GetApiService<SchoolBL>().GetSchoolDetails(SchoolID);
        return schooldetails;
    }

    [HttpGet]
    //[CheckRolePermisionFilter(true, ConstantKeys.WebPermissions.VIEW_WEB_SCHOOL)]
    public async Task<IActionResult> GetVisits()
        => Ok(new { result = await _masterBl.GetApiService<SchoolBL>().GetVisitsAsync() });

   [HttpGet]
    //[CheckRolePermisionFilter(true, ConstantKeys.WebPermissions.VIEW_WEB_SCHOOL)]
    public async Task<IActionResult> GetSchools([FromQuery] SchoolRequest request)
    {
        var result = await _masterBl.GetApiService<SchoolBL>().GetSchools(request);
        return Ok(result);
    }

    [HttpGet]
    [CheckRolePermisionFilter(true, ConstantKeys.WebPermissions.VIEW_WEB_SCHOOL)]
    public async Task<IActionResult> GetSchoolsByDepartment([FromQuery] Guid depId)
    {
        var result = await _masterBl.GetApiService<SchoolBL>().GetSchoolsByDepartmentId(depId);
        return Ok(result);
    }

	[HttpGet]
    [CheckRolePermisionFilter(true, ConstantKeys.WebPermissions.VIEW_WEB_SCHOOL)]
    public async Task<IActionResult> GetSchoolsPlan([FromQuery] SchoolRequest request)
    {
        var result = await _masterBl.GetApiService<SchoolBL>().GetSchoolsPlan(request);
        return Ok(result);
    }
}