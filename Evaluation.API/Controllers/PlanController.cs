using Evaluation.API.Extensions;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API.PlanLayer;
using Evaluation.Services.Models.Planing;
using Evaluation.SharedHelper.Dtos.PlanDto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Evaluation.API.Controllers;

[Route("api/[controller]/[action]")]
public class PlanController(MasterBL masterBL) : ControllerBase
{
    [HttpPost]
    //public async Task<IActionResult> CreateAsync([FromBody] CreateEvaluationPlanDto planRequest)
    public async Task<IActionResult> CreateAsync([FromBody] CreateEvaluationPlanDto planRequest)
    {
        //CreateEvaluationPlanDto? planDto = JsonConvert.DeserializeObject<CreateEvaluationPlanDto>(planRequest);
        //if (planDto == null)
        //return BadRequest(new { error = "Invalid JSON structure." });
        var jsonPlan = await masterBL.GetApiService<PlanServiceRequestServices>().AddEvaulationPlan(planRequest);
        return jsonPlan.ToActionResult();
    }
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
    public async Task<IActionResult> Approve(string approveDto)
    {
        ApproveEvaluationPlanDto? planDto = JsonConvert.DeserializeObject<ApproveEvaluationPlanDto>(approveDto.ToString());
        if (planDto == null)
            return BadRequest(new { error = "Invalid JSON structure." });
        await masterBL.GetApiService<PlanServiceRequestServices>().ApprovePlan(planDto);
        return Ok();
    }
    [HttpPut]
    public async Task<IActionResult> UpdatePlan(Guid id, [FromBody] string planDto)
    {
        var planJson = await masterBL.GetApiService<PlanServiceRequestServices>().UpdatePlanDraft(id, planDto);
        return Ok(planJson);
    }
    [HttpPost]
    public async Task<IActionResult> ApproveDeleteSchool(Guid id, Guid schoolId)
    {
        var deletedSchool = await masterBL.
            GetApiService<PlanServiceRequestServices>()
            .ApproveDeleteSchool(id, schoolId);
        return Ok(deletedSchool);
    }
    [HttpGet]
    public IActionResult GetPlanType()
    {
        return Ok(new { result = GetPlanTypes() });
    }
    private List<PlanTypeDto> GetPlanTypes()
    {
        return new List<PlanTypeDto>
        {
            new PlanTypeDto{Id = 1,Name= "Month"},
            new PlanTypeDto{Id = 2,Name = "Year"}
        };
    }
    public class PlanTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }

}
