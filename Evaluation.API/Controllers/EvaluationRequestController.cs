using Evaluation.DAL.Dtos.Form;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API;
using Evaluation.Services.BusinessLayer.API.FormLayer;
using Evaluation.SharedHelper.Dtos.PlanDto;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.API.Controllers;


[Route("api/[controller]/[action]")]
[ApiController]
public class EvaluationRequestController : ControllerBase
{
    private readonly MasterBL _masterBl;
    public EvaluationRequestController(MasterBL masterBl)
    {
        _masterBl = masterBl;
    }

    [HttpGet]
    public async Task<IActionResult> GetEvaluationRequests([FromQuery] string[] monthes)
    {
        return Ok(await _masterBl.GetApiService<EvaluationRequestBL>().GetEvaluationRequestsForCalender(monthes));
    }

    [HttpPost]
    public async Task<Result<EvaluationRequestCalenderDto>> UpdateEvaluationRequest([FromBody] EvaluationRequestCalenderDto evaluationRequestCalender)
    {
        return await _masterBl.GetApiService<EvaluationRequestBL>().UpdateEvaluationRequest(evaluationRequestCalender);
    }
}
