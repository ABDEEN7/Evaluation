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

    [HttpGet]
    public async Task<Result<List<TeamDto>>> GetTeams()
        => await _masterBl.GetApiService<TeamMemberBL>().GetTeamsAsync();

    [HttpGet]
    public async Task<Result<List<MemberDto>>> GetMembersByTeamId([FromQuery] Guid? teamId)
         => await _masterBl.GetApiService<TeamMemberBL>().GetMembersByTeamId(teamId);

    [HttpGet]
    public async Task<Result<List<ScopeDto>>> GetScopes()
         => await _masterBl.GetApiService<TeamMemberBL>().GetScopesAsync();
    //[HttpGet]
    //public IActionResult GetMebmer()
    //{
    //    var member = _masterBl.GetApiService<>
    //        return Ok(member);
    //}
}
