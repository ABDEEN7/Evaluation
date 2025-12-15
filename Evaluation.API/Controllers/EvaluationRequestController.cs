using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API;
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
    public async Task<IActionResult> GetEvaluationRequests()

    {
        return Ok(await _masterBl.GetApiService<EvaluationRequestBL>().GetEvaluationRequestsForCalender());
    }
}
