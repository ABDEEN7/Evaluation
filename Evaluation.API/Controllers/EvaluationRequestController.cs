using Evaluation.DAL.Dtos.Form;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API;
using Evaluation.Services.BusinessLayer.API.FormLayer;
using Evaluation.Services.Models.API;
using Evaluation.SharedHelper.Dtos.PlanDto;
using Evaluation.SharedHelper.Models.Api.ServiceRequestEntitiesDTO;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.API.Controllers;


[ApiController]
[Route("api/[controller]/{depRouting}/[action]")]
[Authorize]

public class EvaluationRequestController : ControllerBase
{
    private readonly MasterBL _masterBl;
    public EvaluationRequestController(MasterBL masterBl)
    {
        _masterBl = masterBl;
    }

    [HttpGet]
    //[CheckRolePermisionFilter(true, ConstantKeys.WebPermissions.VIEW_EVALUATION_REQUEST)]
    public async Task<IActionResult> GetEvaluationRequests([FromQuery] string[] monthes)
    {
        return Ok(await _masterBl.GetApiService<EvaluationRequestBL>().GetEvaluationRequestsForCalender(monthes));
    }

    [HttpPost]
    //[CheckRolePermisionFilter(true, ConstantKeys.WebPermissions.VIEW_EVALUATION_REQUEST)]
    public async Task<WebAppEvaluationRequestsDTO> GetEvaluationRequestsBySchoolId([FromQuery] Guid Id, [FromBody] FilterRequestsDTO model)
    {
        return await _masterBl.GetApiService<EvaluationRequestBL>().GetEvaluationRequestsByOrgTreeId(Id, model);
    }

    [HttpPost]
    //[CheckRolePermisionFilter(true, ConstantKeys.WebPermissions.VIEW_EVALUATION_REQUEST)]
    public async Task<Result<EvaluationRequestCalenderDto>> UpdateEvaluationRequest([FromBody] EvaluationRequestCalenderDto evaluationRequestCalender)
    {
        return await _masterBl.GetApiService<EvaluationRequestBL>().UpdateEvaluationRequest(evaluationRequestCalender);
    }

    [HttpPost]
    //[CheckRolePermisionFilter(true, ConstantKeys.WebPermissions.UPDATE_EVALUATION_REQUEST)]
    public async Task<Result<EvaluationRequestCalenderDto>> UpdateEvaluationServiceRequest([FromBody] EvaluationRequestCalenderDto evaluationRequestCalender)
    {
        return await _masterBl.GetApiService<EvaluationServiceRequestBL>().UpdateEvaluationServiceRequestRequest(evaluationRequestCalender);
    }
    //[HttpPost]
    //[CheckRolePermisionFilter(true, ConstantKeys.WebPermissions.VIEW_EVALUATION_REQUEST)]
    //public IActionResult DeleteEvaluationRequest(Guid id)
    //{
    //    return await _masterBl.GetApiService<EvaluationServiceRequestBL>().DeleteEvaluationRequest(id);

    //}
}
