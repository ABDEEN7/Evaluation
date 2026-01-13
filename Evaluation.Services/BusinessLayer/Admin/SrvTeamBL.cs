using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Models.Planing.TeamsModule;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Models.Admin;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.Admin;

public class SrvTeamBL : AdminBase
{
    public SrvTeamBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceScopeFactory serviceScopeFactory, RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
    {
    }
    public async Task<List<TeamDto>> GetTeamList(int page, int pageSize)
    {
        var list = await uow.GetRepository<Team>()
            .GetAllActiveNonDeleted()
            //.Include(x => x.CreateBy)
            //.OrderByDescending(x => x.OrderNo)
            //.ThenByDescending(x => x.CreateDate)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync();
        var result = mapper.Map<List<TeamDto>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);
        return result;
    }
    public async Task<TeamDto> SaveTeam(TeamDto dto)
    {
        var team = new Team
        {
            NameAr = dto.NameAr,
            NameEn = dto.NameEn,
            DepartmentId = dto.DepartmentId,
            IsActive = true,
            UserTeams = new List<UserTeam>()
        };
        foreach (var user in dto.Users)
        {
            var userTeam = new UserTeam
            {
                UserId = user.UserId,
                UserTeamScope = new List<UserTeamScope>()
            };
            foreach (var scopeId in user.ScopeIds)
            {
                userTeam.UserTeamScope.Add(new UserTeamScope
                {
                    ScopeId = scopeId
                });
            }
            team.UserTeams.Add(userTeam);
        }
        uow.GetRepository<Team>().Insert(team);
        await uow.CommitAsync();
        dto.ResponseStatus = DBResult.Updated;
        return dto;
    }
    public async Task<TeamDto> DeleteTeamAsync(Guid? id)
    {
        var repository = uow.GetRepository<Team>();
        var Team = await repository
            .GetAllActiveNonDeleted(x => x.Id == id)
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);
        if (Team == null)
        {
            return new TeamDto
            {
                ResponseStatus = DBResult.NotFound
            };
        }
        repository.Delete(Team);
        await uow.CommitAsync();
        var result = mapper.Map<TeamDto>(Team, opts =>
        opts.Items["Language"] = _requestInfo.Lang);
        result.ResponseStatus = DBResult.Deleted;
        return result;
    }
    public async Task<TeamDto> UpdateTeam(TeamDto Team)
    {
        if (Team == null)
        {
            return new TeamDto
            {
                ResponseStatus = DBResult.Error
            };
        }
        if (Team.Id == null)
        {
            return new TeamDto
            {
                ResponseStatus = DBResult.Error
            };
        }
        var repository = uow.GetRepository<Team>();
        var team = await uow
            .GetRepository<Team>()
            .GetAllActiveNonDeleted()
            .FirstOrDefaultAsync(x => x.Id == Team.Id);
        team.NameAr = Team.NameAr;
        team.NameEn = Team.NameEn;
        team.IsActive = Team.IsActive;
        team.UpdateById = userInfo.UserId;
        team.UpdateDate = DateTime.UtcNow;

        repository.Update(team);
        await uow.CommitAsync().ConfigureAwait(false);
        var result = mapper.Map<TeamDto>(team);
        result.ResponseStatus = DBResult.Updated;
        return result;
    }
    //public async Task<bool> UpdateTeamOrderAsync(List<OrderingDTO> message)
    //{
    //    var Teams = await uow.GetRepository<Team>()
    //        .GetAllActiveNonDeleted()
    //        .Where(n => message.Select(m => m.Id).Contains(n.Id))
    //        .ToListAsync();
    //    foreach (var Team in Teams)
    //    {
    //        Team.OrderNo = message.First(m => m.Id == Team.Id).OrderNo;
    //    }
    //    await uow.CommitAsync();
    //    return true;
    //}
}

