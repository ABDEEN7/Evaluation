using Evaluation.API.Extensions;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API.PlanLayer;
using Evaluation.Services.BusinessLayer.API.SteamerLayer;
using Evaluation.Services.Models.Planing;
using Evaluation.SharedHelper.Dtos.PlanDto;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.API.Controllers;

[Route("api/[controller]/[action]")]
public class PlanController(MasterBL masterBL) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetPlanById(Guid planId)
    {
        var plan = await masterBL.GetApiService<PlanServiceRequestServices>().GetPlanByIdAsync(planId);
        return Ok(new { result = plan });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEvaluationPlanDto planRequest)
    {
        var jsonPlan = await masterBL.GetApiService<PlanServiceRequestServices>().AddEvaulationPlan(planRequest);
        return jsonPlan.ToActionResult();
    }
    //[HttpPost]
    //public async Task<IActionResult> CreatePlan([FromBody] CreateEvaluationPlanDto planApproved)
    //{
    //    var plan = await masterBL.GetAdminService<PlanServiceRequestServices>().AddEvaulationPlan
    //}
    //[HttpDelete]
    //public async Task<IActionResult> DeletePlan(Guid id)
    //{
    //    var isDeleted = await masterBL.GetApiService<PlanServiceRequestServices>().DeletePlanDraft(id);
    //    return isDeleted.ToActionResult();
    //}

    [HttpPost]
    public IActionResult RequestDeleteSchool(RequestDeleteSchoolDto requestDelete)
    {
        return Ok();
    }
    [HttpPost]
    public IActionResult ApproveDeleteSchoolFromPlan(Guid requestId)
    {
        //var
        return Ok();
    }
    [HttpPost]
    public async Task<IActionResult> Approve([FromBody] CreateEvaluationPlanDto approveDto)
    {
        await masterBL.GetApiService<PlanServiceRequestServices>().ApprovePlan(approveDto);
        return Ok();
    }
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdatePlan(Guid id, [FromBody] UpdatePlanDto planDto)
    {
        var result = await masterBL
              .GetApiService<PlanServiceRequestServices>()
              .UpdatePlanAsync(id, planDto);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetSemesters()
    {
        //var semester = await masterBL.GetApiService<SemesterRequestServices>().GetSemestersAsync(new Guid("37689d34-4928-4bb9-92b4-8a11abc0dbaf"));
        var semester = await masterBL.GetApiService<SemesterRequestServices>().GetSemestersAsync();
        return Ok(new { result = semester });
        //return semester.ToActionResult();
    }
}
