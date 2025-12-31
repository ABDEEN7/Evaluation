using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Models.Planing.TeamsModule;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.TeamMemberDto;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Evaluation.SharedHelper.Models.Api;
using FluentResults;
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
    public async Task<Result<List<TeamDto>>> GetTeamsAsync()
    {
        var team = await teamMemberService.GetTeamAsync();
        return mapper.Map<List<TeamDto>>(team);
    }
    public async Task<Result<List<MemberDto>>> GetMembersByTeamId(Guid? teamId)
    {
        var member = unitOfWork.GetRepository<MinistryUser>()
               .GetAllActiveNonDeleted();
        if (teamId != null)
                member = member.Where(x => x.UserTeams.Any(t => t.TeamId == teamId));

        var memberTeamDto = mapper.Map<List<MemberDto>>(member.ToList());
        return memberTeamDto;
    }
    public async Task<List<Team>> GetTeams()
    {
        var departmentId = await unitOfWork.GetRepository<Department>()
                                .GetAllActiveNonDeleted(d => d.UserDepartments.Any(ud => ud.UserId == userInfo.UserId))
                                .OrderByDescending(d => d.UserDepartments
                            .Where(ud => ud.UserId == userInfo.UserId)
                            .Max(ud => ud.CreateDate)).Select(x => x.Id).FirstOrDefaultAsync();
        var teams = await unitOfWork
                        .GetRepository<Team>()
                        .GetAllActiveNonDeleted(x => x.DepartmentId == departmentId)
                        .ToListAsync();
        return teams;
    }
    public async Task<List<ScopeDto>> GetScopesAsync()
    {
        var scopes = await teamMemberService.GetScopes();
        var scopesDto = mapper.Map<List<ScopeDto>>(scopes);
        return scopesDto;
    }
}
