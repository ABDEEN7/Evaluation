using Azure.Core;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API.TeamMemberBL;
using Evaluation.SharedHelper.Dtos.TeamMemberDto;
using Evaluation.SharedHelper.Models.Admin;
using Evaluation.SharedHelper.Models.Api;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.API.Controllers;

[Route("api/[controller]/[action]")]
public class TeamMemberController : ControllerBase
{
    private readonly MasterBL _masterBl;

    public TeamMemberController(MasterBL masterBl)
    {
        _masterBl = masterBl;
    }
    //public async Task<Result><SelectedTeamMember> GetTeamMeberById(Guid evaluationRequestAssignmentId)
    //{

    //}

    [HttpGet]
    public async Task<Result<TeamMembersResponse>> GetTeams()
        => await _masterBl.GetApiService<TeamMemberBL>().GetTeamsAsync();

    [HttpGet]
    public async Task<Result<List<MemberDto>>> GetMembersByTeam([FromQuery] Guid? teamId)
         => await _masterBl.GetApiService<TeamMemberBL>().GetMembersByTeamId(teamId);

    [HttpGet]
    public async Task<Result<List<ScopeDto>>> GetScopes()
         => await _masterBl.GetApiService<TeamMemberBL>().GetScopesAsync();

    [HttpGet]
    public Task<Result<List<EvaluationRequestAssignmentDto>>> GetTeamMemberByEvaluationRequest(Guid evaluationRequestId)
    {
        var member = _masterBl.GetApiService<TeamMemberBL>().GetTeamByEvaluationRequestId(evaluationRequestId);
        return member;
    }
}
