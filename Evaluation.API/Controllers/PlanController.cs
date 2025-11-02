using Evaluation.API.Extensions;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API.PlanLayer;
using Evaluation.SharedHelper.Dtos.PlanDto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Evaluation.API.Controllers;

[Route("api/[controller]/[action]")]
public class PlanController(MasterBL masterBL) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] string planRequest)
    {

        CreateEvaluationPlanDto? planDto = JsonConvert.DeserializeObject<CreateEvaluationPlanDto>(planRequest.ToString());
        if (planDto == null)
            return BadRequest(new { error = "Invalid JSON structure." });
        var jsonPlan = await masterBL.GetApiService<PlanServiceRequestServices>().AddEvaulationPlan(planDto);
        return Ok(jsonPlan);
    }
    //[HttpDelete]
    //public async Task<IActionResult> DeletePlan(Guid id)
    //{
    //    var isDeleted = await masterBL.GetApiService<PlanServiceRequestServices>().DeletePlanDraft(id);
    //    return isDeleted.ToActionResult();
    //}
    [HttpPost]
    public IActionResult RequestDeleteSchool(Guid planId, Guid schoolId)
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
    public IActionResult UpdatePlan(Guid id, [FromBody] string planDto)
    {

        return Ok();
    }
    [HttpPost]
    public IActionResult ApproveDeleteSchool(Guid id, Guid schoolId)
    {
        return Ok();
    }
    [HttpGet]
    public IActionResult GetPlanType()
    {
        return Ok(GetPlanTypes());
    }
    private List<PlanTypeDto> GetPlanTypes()
    {
        return new List<PlanTypeDto>
        {
            new PlanTypeDto{Id = new Guid(),Name= "Month"},
            new PlanTypeDto{Id = new Guid(),Name = "Year"}
        };
    }
    public class PlanTypeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }

}
