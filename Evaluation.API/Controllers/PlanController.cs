
using Evaluation.API.ActionFilter;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API.PlanLayer;
using Evaluation.Services.BusinessLayer.API.SteamerLayer;
using Evaluation.SharedHelper.Dtos.PlanDto;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Evaluation.API.Controllers;

[Route("api/[controller]/{depRouting}/[action]")]
public class PlanController(MasterBL masterBL) : ControllerBase
{

    [HttpPost]
    [CheckRolePermisionFilter(true, ConstantKeys.WebPermissions.ADD_WEB_PLAN)]
    public async Task<IActionResult> InsertOrUpdatePlan([FromBody] CreateEvaluationPlanDto approveDto)
    {
        await masterBL.GetApiService<PlanServiceRequestServices>().InsertOrUpdatePlan(approveDto);
        return Ok(new { success = true });
    }


    [HttpGet]
    [CheckRolePermisionFilter(true, ConstantKeys.WebPermissions.GET_SEMESTERS_REQUEST)]
    public async Task<IActionResult> GetSemesters()
    {
        var semester = await masterBL.GetApiService<SemesterRequestServices>().GetSemestersAsync();
        return Ok(new { result = semester });
    }
    [HttpGet]
    [CheckRolePermisionFilter(true, ConstantKeys.WebPermissions.GET_WEB_PLAN_DETAILS_REQUEST)]
    public async Task<IActionResult> GetPlanDetails(Guid planId)
    {
        var plan = await masterBL.GetApiService<PlanServiceRequestServices>().GetPlanByIdAsync(planId);
        return Ok(new { result = plan });
    }

    [HttpGet("{planId:guid}")]
    [CheckRolePermisionFilter(true, ConstantKeys.WebPermissions.GET_WEB_PLAN_DETAILS_REQUEST)]
    public async Task<IActionResult> GetPlansWithunSelectedSchoolsDetails(Guid planId)
    {
        var plan = await masterBL.GetApiService<PlanServiceRequestServices>().GetPlanWithSchoolsByIdAsync(planId);
        return Ok(new { result = plan });
    }

    [HttpGet]
    [CheckRolePermisionFilter(true, ConstantKeys.WebPermissions.VIEW_WEB_PLAN)]
    public async Task<IActionResult> GetPlans([FromQuery] PlanDetailsRequestDto request)
    {
        var result = Ok(await masterBL
            .GetApiService<PlanServiceRequestServices>()
            .GetPlansAsync(request));
        return result;
    }
    [HttpPost]
    public async Task<Result<ValidationResult>> ValidateEvaluationPlan([FromBody] CreateEvaluationPlanDto planDtoRequest)
    {
        return await masterBL.GetApiService<PlanServiceRequestServices>().ValidateEvaluationPlan(planDtoRequest);
    }
    [HttpPost]
    [CheckRolePermisionFilter(true, ConstantKeys.WebPermissions.DELETE_WEB_PLAN)]
    public async Task<IActionResult> DeletePlan(Guid id)
    {
        await masterBL.GetApiService<PlanServiceRequestServices>().DeletePlanById(id);
        return Ok();
    }
    [HttpGet]
    [CheckRolePermisionFilter(true, ConstantKeys.WebPermissions.VIEW_WEB_PLAN)]
    public async Task<IActionResult> GetPlansDDL()
    {
        return Ok(await masterBL.GetApiService<PlanServiceRequestServices>().GetPlans());
    }
    [HttpGet]
    [CheckRolePermisionFilter(true, ConstantKeys.WebPermissions.VIEW_WEB_PLAN)]
    public async Task<IActionResult> GetFomrEvalMatrixValueList()
    {
        var result = await masterBL.GetApiService<PlanServiceRequestServices>().GetFomrEvalMatrixValueList();
        return Ok(new { result = result });
    }
    [HttpGet]
    [CheckRolePermisionFilter(true, ConstantKeys.WebPermissions.VIEW_WEB_PLAN)]
    public async Task<IActionResult> GetDepartmentConfig()
    {
        var result = await masterBL.GetApiService<PlanServiceRequestServices>().GetDepartmentConfigsAsync();
        return Ok(new { result = result });
    }

}