using Evaluation.API.ActionFilter;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API.ReassignLayer;
using Evaluation.SharedHelper.Dtos.TeamMemberDto.ReassignDto;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models;
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
    //[CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.WebPermissions.VIEW_WEB_ReassignAssignment })]
    [HttpGet]
    public async Task<IActionResult> GetEvalFormItemLists(Guid? userId)
    {
        var response = await _masterBl.GetApiService<ReassignBL>()
            .GetReAssignedDropList(userId);
        return Ok(new ResponseEntity(response));
    }
    //[CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.WebPermissions.VIEW_WEB_ReassignAssignment })]
    [HttpGet]
    public async Task<IActionResult> GetUserAssignments(Guid userId)
    {
        var response = await _masterBl.GetApiService<ReassignBL>()
            .GetEvaluationUserAssignments(userId);
        return Ok(new ResponseEntity(response));
    }
    //[CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.WebPermissions.ADD_WEB_ReassignAssignment })]
    [HttpPost]
    public async Task<ResponseDto> ReassignEvaluationRequestUser(
    [FromBody] RequestReassignDto request)
    {
        return await _masterBl.GetApiService<ReassignBL>()
            .UpdateReassignEvaluationRequestUser(request);
    }

}
