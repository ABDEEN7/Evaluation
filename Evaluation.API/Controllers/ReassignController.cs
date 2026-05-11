using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API.EvaluationForm;
using Evaluation.Services.BusinessLayer.API.ReassignLayer;
using Evaluation.SharedHelper.Dtos.TeamMemberDto.ReassignDto;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.API.Controllers;

public class ReassignController : ControllerBase
{
    private readonly MasterBL _masterBl;

    public ReassignController(MasterBL masterBL)
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
        var response =await _masterBl.GetApiService<ReassignBL>()
            .GetReAssignedDropList(userId);
        return Ok(new ResponseEntity(response));
    }
}
