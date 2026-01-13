using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.TeamMemberDto;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api;
using FluentResults;
using HarfBuzzSharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.API.TeamMemberBL;

public class TeamMemberBL(IServiceScopeFactory serviceScopeFactory,
    CacheDataProvider cacheDataProvider,
    UnitOfWork unitOfWork,
    LoggingServices loggingServices,
    IMapper mapper,
    UserInfo userInfo,
    IServiceProvider serviceProvider,
    RequestInfo requestInfo,
    TeamMemberService teamMemberService
    ) : ApiBase(serviceScopeFactory, cacheDataProvider, unitOfWork, loggingServices, mapper, userInfo,
        serviceProvider, requestInfo)
{
    //public async Task<List<Team>> GetTeamsMembers()
    //{
    //    var departmentId = await unitOfWork.GetRepository<Department>()
    //                            .GetAllActiveNonDeleted(d => d.UserDepartments.Any(ud => ud.UserId == userInfo.UserId))
    //                            .OrderByDescending(d => d.UserDepartments
    //                        .Where(ud => ud.UserId == userInfo.UserId)
    //                        .Max(ud => ud.CreateDate)).Select(x => x.Id).FirstOrDefaultAsync();
    //    var teams = await unitOfWork
    //                    .GetRepository<Team>()
    //                    .GetAllActiveNonDeleted(x => x.DepartmentId == departmentId)
    //                    .Include(x=>x.UserTeams)
    //                    .ToListAsync();
    //    return teams;
    //}
    public async Task<Result<TeamMembersResponse>> GetTeamsAsync()
    {
        var team = await teamMemberService.GetTeamAsync();
        bool isNdaActive = await
          unitOfWork
          .GetRepository<Department>()
          .GetAllActiveNonDeleted(d => d.IsNDA && d.UserDepartments.Any(ud => ud.UserId == userInfo.UserId))
          .OrderByDescending(x => x.CreateDate)
          .AnyAsync();
        var teamRespons = mapper.Map<List<TeamDto>>(team);
        TeamMembersResponse teamMembers = new TeamMembersResponse
        {
            Data = teamRespons,
            IsNDA = true
        };
        return teamMembers;
    }
    public async Task<Result<List<MemberDto>>> GetMembersByTeamId(Guid? teamId)
    {
        var member = unitOfWork.GetRepository<MinistryUser>()
               .GetAllActiveNonDeleted();
        if (teamId != null)
            member = member.Where(x => x.UserTeams!.Any(t => t.TeamId == teamId));

        var memberWithParty = await member
            .Include(x => x.UserPartTypes!)
            .ThenInclude(w => w.PartyType).ToListAsync();
        var memberTeamDto = mapper.Map<List<MemberDto>>(memberWithParty);
        return memberTeamDto;
    }
    public async Task<List<ScopeDto>> GetScopesAsync()
    {
        var scopes = await teamMemberService.GetScopes();
        var scopesDto = mapper.Map<List<ScopeDto>>(scopes);
        return scopesDto;
    }
    public async Task<Result<List<EvaluationRequestAssignmentDto>>> GetTeamByEvaluationRequestId(Guid evaluationRequestId)
    {
        var team = unitOfWork
            .GetRepository<EvaluationRequestAssignment>()
            .GetAllActiveNonDeleted(x => x.EvaluationRequestId == evaluationRequestId)
            .Include(s => s.EvalRequestAssignmentScopies)
            .ToList();
        var evaluationRequestAssignmentDto = mapper.Map<List<EvaluationRequestAssignmentDto>>(team);
        return evaluationRequestAssignmentDto;
    }

}
