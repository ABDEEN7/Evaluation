using Evaluation.API.Extensions;
using Evaluation.DAL.Dtos;
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
    public async Task<IActionResult> Create([FromBody] CreateEvaluationPlanDto planRequest)
    {
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
    //[HttpPost]
    //public async Task<IActionResult> Approve(string approveDto)
    //{
    //    ApproveEvaluationPlanDto? planDto = JsonConvert.DeserializeObject<ApproveEvaluationPlanDto>(approveDto.ToString());
    //    if (planDto == null)
    //        return BadRequest(new { error = "Invalid JSON structure." });
    //    await masterBL.GetApiService<PlanServiceRequestServices>().ApprovePlan(planDto);
    //    return Ok();
    //}
    [HttpPut]
    public async Task<IActionResult> UpdatePlan(Guid id, [FromBody] string planDto)
    {
        var planJson = await masterBL.GetApiService<PlanServiceRequestServices>().UpdatePlanDraft(id, planDto);
        return Ok(planJson);
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

    public IActionResult GetSemesters()
    {
        return Ok(new { result = GetSemestersAsync() });
    }
    private List<SemesterDto> GetSemestersAsync()
    {
        return new List<SemesterDto>
    {
        new SemesterDto { Id = Guid.Parse("11111111-2111-1111-1111-111111111111"), Name = "Firstly",StartDate = new DateTime(2026,04,01), EndDate= new DateTime(2026,08,01) },
        new SemesterDto { Id = Guid.Parse("22222222-1222-2222-2222-222222222222"), Name = "Scandle", StartDate = new DateTime(2025,04,01), EndDate= new DateTime(2025,08,01)  }

    };
    }

    public class PlanTypeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string BackendName { get; set; }
    }

}
