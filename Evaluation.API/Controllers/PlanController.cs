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
    //[HttpPost]
    //public async Task<IActionResult> Approve(string approveDto)
    //{
    //    ApproveEvaluationPlanDto? planDto = JsonConvert.DeserializeObject<ApproveEvaluationPlanDto>(approveDto.ToString());
    //    if (planDto == null)
    //        return BadRequest(new { error = "Invalid JSON structure." });
    //    await masterBL.GetApiService<PlanServiceRequestServices>().ApprovePlan(planDto);
    //    return Ok();
    //}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdatePlan(Guid id, [FromBody] UpdatePlanDto planDto)
    {
        var result = await masterBL
              .GetApiService<PlanServiceRequestServices>()
              .UpdatePlanAsync(id, planDto);
        return Ok(result);
    }
    [HttpPost]
    //public async Task<IActionResult> ApproveDeleteSchool(Guid id, Guid schoolId)
    //{
    //    var deletedSchool = await masterBL.
    //        GetApiService<PlanServiceRequestServices>()
    //        .ApproveDeleteSchool(id, schoolId);
    //    return Ok(deletedSchool);
    //}
    [HttpGet]
    public IActionResult GetPlanType()
    {
        return Ok(new { result = GetPlanTypes() });
    }
    private List<PlanTypeDto> GetPlanTypes()
    {
        return new List<PlanTypeDto>
    {
        new PlanTypeDto { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Month", BackendName = "Month" },
        new PlanTypeDto { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Year", BackendName = "Year" },
        new PlanTypeDto { Id = Guid.Parse("33333333-2222-2222-2222-222222222222"), Name = "Semester" , BackendName = "Semester"},
        new PlanTypeDto { Id = Guid.Parse("44444444-2222-2222-2222-222222222222"), Name = "Custom" , BackendName = "Custom"}
    };
    }
    [HttpGet]
    public async Task<IActionResult> GetSemesters()
    {
        //var semester = await masterBL.GetApiService<SemesterRequestServices>().GetSemestersAsync(new Guid("37689d34-4928-4bb9-92b4-8a11abc0dbaf"));
        var semester = await masterBL.GetApiService<SemesterRequestServices>().GetSemestersAsync();
        return Ok(new { result = semester });
        //return semester.ToActionResult();
    }

    public class PlanTypeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string BackendName { get; set; }
    }


}
