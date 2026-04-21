using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API.TeamMemberBL;
using Evaluation.SharedHelper.Dtos.TeamMemberDto;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.API.Controllers;

[Route("api/[controller]/{depRouting}/[action]")]
public class AssignmentController : ControllerBase
{
    private readonly MasterBL _masterBl;

    public AssignmentController(MasterBL masterBl)
    {
        _masterBl = masterBl;
    }
    //public async Task<Result><SelectedAssignment> GetTeamMeberById(Guid evaluationRequestAssignmentId)
    //{

    //}

    [HttpGet]
    public async Task<Result<AssignmentsResponse>> GetTeams()
        => await _masterBl.GetApiService<AssignmentBL>().GetTeamsAsync();

    [HttpGet]
    public async Task<Result<List<AssignmentDto>>> GetMembersByTeam([FromQuery] Guid? teamId)
         => await _masterBl.GetApiService<AssignmentBL>().GetMembersByTeamId(teamId);

    [HttpGet]
    public async Task<Result<List<ScopeDto>>> GetScopes()
         => await _masterBl.GetApiService<AssignmentBL>().GetScopesAsync();
    [HttpPost]
    public async Task<Result<bool>> AddEvaluationRequestAssignment(Guid evaluationRequestId, [FromBody] List<EvalTeamRequestDto> model)
                 => await _masterBl.GetApiService<AssignmentBL>().AddedRequestAssignment(evaluationRequestId, model);
    [HttpGet]
    public Task<Result<List<EvaluationRequestAssignmentDto>>> GetAssignmentByEvaluationRequest(Guid evaluationRequestId)
    {
        var member = _masterBl.GetApiService<AssignmentBL>().GetTeamByEvaluationRequestId(evaluationRequestId);
        return member;
    }
    [HttpPost]
    public async Task<Result<ValidationResult>> ValidationEvaluationRequestAssignment(Guid evaluationRequestId, [FromBody] List<EvalTeamRequestDto> model)
                => await _masterBl.GetApiService<AssignmentBL>().AssignmentValidationResult(evaluationRequestId, model);

    [HttpPost]
    public async Task<Result<bool>> SendMail(Guid userId)
    {
       var result = await _masterBl.GetApiService<AssignmentBL>().SendMailUser(userId);
        return Ok(result);
    }
}
