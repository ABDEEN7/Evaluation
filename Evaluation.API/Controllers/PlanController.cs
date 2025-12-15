using Evaluation.API.ActionFilter;
using Evaluation.API.Extensions;
using Evaluation.DAL;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API.PlanLayer;
using Evaluation.Services.BusinessLayer.API.SteamerLayer;
using Evaluation.SharedHelper.Dtos.PlanDto;
using Evaluation.SharedHelper.Dtos.SchoolDto;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.API.Controllers;

[Route("api/[controller]/[action]")]
public class PlanController(MasterBL masterBL) : ControllerBase
{

    [HttpPost]
    //[CheckRolePermisionFilter(true, ConstantKeys.WebPermission.APPROVE_WEB_PLAN_REQUEST)]
    public async Task<IActionResult> InsertOrUpdatePlan([FromBody] CreateEvaluationPlanDto approveDto)
    {
        await masterBL.GetApiService<PlanServiceRequestServices>().InsertOrUpdatePlan(approveDto);
        return Ok();
    }


    [HttpPut("{id:guid}")]
    //[CheckRolePermisionFilter(true, ConstantKeys.WebPermission.UPDATE_WEB_PLAN_REQUEST)]
    public async Task<IActionResult> UpdatePlan(Guid id, [FromBody] UpdatePlanDto planDto)
    {
        var result = await masterBL
              .GetApiService<PlanServiceRequestServices>()
              .UpdatePlanAsync(id, planDto);
        return Ok(result);
    }

    [HttpGet]
    //[CheckRolePermisionFilter(true, ConstantKeys.WebPermission.GET_SEMESTERS_REQUEST)]
    public async Task<IActionResult> GetSemesters()
    {
        //var semester = await masterBL.GetApiService<SemesterRequestServices>().GetSemestersAsync(new Guid("37689d34-4928-4bb9-92b4-8a11abc0dbaf"));
        var semester = await masterBL.GetApiService<SemesterRequestServices>().GetSemestersAsync();
        return Ok(new { result = semester });
        //return semester.ToActionResult();
    }
    [HttpGet("{planId:guid}")]
    //[CheckRolePermisionFilter(true, ConstantKeys.WebPermission.GET_WEB_PLAN_DETAILS_REQUEST)]
    //public async Task<IActionResult> GetPlanDetails(Guid planId)
    //{
    //    var plan = await masterBL.GetApiService<PlanServiceRequestServices>().GetPlanByIdAsync(planId);
    //    return Ok(new { result = plan });
    //}
    [HttpGet]
    public async Task<IActionResult> GetPlanDetails(Guid planId)
    {
        // Mock data
        var plan = new PlanDetailsDto
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000000"),
            Name = "three schools",
            StartDate = DateOnly.Parse("2025-11-01"),
            EndDate = DateOnly.Parse("2025-11-30"),
            PlanTypeId = Guid.Parse("ea407d0f-45d2-495b-a51e-665f27e013f6"),
            Schools = new List<SelectedSchool>
        {
            new SelectedSchool
            {
                Id = Guid.Parse("f01ca4c4-dc39-4558-8a6c-0719e07b20d4"),
                Name = "first",
                StartEvaluationDate = DateTime.Parse("2025-11-01T00:00:00"),
                EndEvaluationDate = DateTime.Parse("2025-11-03T00:00:00"),
                VisitTypeId = Guid.Parse("b19d3a0e-2424-4174-8b9a-1236faa8c241"),
                IsActive = null,
                CreateById = null,
                CreateDate = DateTime.MinValue,
                UpdateById = null,
                UpdateDate = null,
                DeleteById = null,
                IsDeleted = null
            },
            new SelectedSchool
            {
                Id = Guid.Parse("160c62a6-0556-4a33-b6c6-0ad687062492"),
                Name = "second",
                StartEvaluationDate = DateTime.Parse("2025-11-04T00:00:00"),
                EndEvaluationDate = DateTime.Parse("2025-11-07T00:00:00"),
                VisitTypeId = Guid.Parse("e56666bf-b6ea-4063-a169-282aadb9fbe3"),
                IsActive = null,
                CreateById = null,
                CreateDate = DateTime.MinValue,
                UpdateById = null,
                UpdateDate = null,
                DeleteById = null,
                IsDeleted = null
            },
            new SelectedSchool
            {
                Id = Guid.Parse("4ecb31d7-db11-4b46-8b0f-bca1d0497f96"),
                Name = "third",
                StartEvaluationDate = DateTime.Parse("2025-11-14T00:00:00"),
                EndEvaluationDate = DateTime.Parse("2025-11-16T00:00:00"),
                VisitTypeId = Guid.Parse("e56666bf-b6ea-4063-a169-282aadb9fbe3"),
                IsActive = null,
                CreateById = null,
                CreateDate = DateTime.MinValue,
                UpdateById = null,
                UpdateDate = null,
                DeleteById = null,
                IsDeleted = null
            }
        },
            AcademicYearId = Guid.Parse("c7bf1856-34ab-4fbf-a7b0-2e5471001868"),
            PlanStatusId = Guid.Parse("de265dcc-a904-4921-ac5b-a65d9304ccf8"),
            SemesterId = null
        };

        return Ok(new { result = plan });
    }
    [HttpGet]
    public async Task<IActionResult> GetPlans(PlanRequestDto requestDto)
    {
        return Ok(await masterBL
            .GetApiService<PlanServiceRequestServices>()
            .GetPlansAsync(requestDto));
    }
}