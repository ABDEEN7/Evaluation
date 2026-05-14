using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API.ReassignLayer;
using Evaluation.SharedHelper.Dtos.TeamMemberDto.ReassignDto;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.API.Controllers;
[Route("api/[controller]/{depRouting}/[action]")]
public class ReassignAssignmentController : ControllerBase
{
    private readonly MasterBL _masterBl;

    public ReassignAssignmentController(MasterBL masterBL)
    {
        _masterBl = masterBL;
    }
    [HttpGet]
    public async Task AddReassignEvaluationRequest(RequestReassignDto result)
    {
        //var result =_masterBL.GetApiService<R>
    }
    [HttpGet]
    public async Task<IActionResult> GetEvalFormItemLists(Guid? userId)
    {
        var response = await _masterBl.GetApiService<ReassignBL>()
            .GetReAssignedDropList(userId);
        return Ok(new ResponseEntity(response));
    }
    [HttpGet]
    public async Task<IActionResult> GetUserAssignments(Guid userId)
    {
        var response = await _masterBl.GetApiService<ReassignBL>()
            .GetEvaluationUserAssignments(userId);
        return Ok(new ResponseEntity(response));
    }
    [HttpPost]
    public async Task<IActionResult> UpdateReassignEvaluationRequestUser(RequestReassignDto request)
    {
        await _masterBl.GetApiService<ReassignBL>()
            .UpdateReassignEvaluationRequestUser(request);
        return Ok();
    }

}
