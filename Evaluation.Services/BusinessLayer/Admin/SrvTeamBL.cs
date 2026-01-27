using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Models.Planing.TeamsModule;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.DAL.Models.Website;
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
            .GetAllNonDeleted()
            .Include(x => x.CreateBy)
            .OrderByDescending(x => x.CreateDate)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync();
        var result = mapper.Map<List<TeamDto>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);
        return result;
    }

    public async Task<List<UserTeamScopeDetailDTO>> GetAllUserTeamScopeList(Guid TeamId)
    {
        var result = await uow.GetRepository<UserTeam>()
    .GetAllNonDeleted()
    .Where(x => x.TeamId == TeamId)
    .GroupJoin(
        uow.GetRepository<UserTeamScope>().GetAllNonDeleted(),
        ut => ut.Id,
        uts => uts.UserTeamId,
        (ut, scopes) => new UserTeamScopeDetailDTO
        {
            Id=ut.Id,
            Scope = scopes.Select(s => s.ScopeId).ToArray(),
            User = ut.UserId,
            IsActive = ut.IsActive,
            
        })
    .ToListAsync();
        return result;
    }
    public async Task<List<DropdownItem>> GetUserList()
    {

        var result = await uow.GetRepository<MinistryUser>()
                    .GetAllActiveNonDeleted()
                    .Select(x=>new DropdownItem
                    {
                        Id=x.Id,
                        Name=_requestInfo.Lang=="ar"?x.NameAr:x.NameEn
                    })
                    .ToListAsync();
        return result;
    }
    public async Task<List<DropdownItem>> GetScopeList()
    {

        var result = await uow.GetRepository<Scope>()
                    .GetAllActiveNonDeleted()
                    .Select(x=>new DropdownItem
                    {
                        Id=x.Id,
                        Name=_requestInfo.Lang=="ar"?x.NameAr:x.NameEn
                    })
                    .ToListAsync();
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
           // UserTeams = new List<UserTeam>()
        };
       
        uow.GetRepository<Team>().Insert(team);
        await uow.CommitAsync();
        var result = mapper.Map<TeamDto>(team, opts => opts.Items["Language"] = _requestInfo.Lang);
        result.ResponseStatus = DBResult.Inserted;
        return result;
    }
    public async Task<TeamDto> DeleteTeamAsync(Guid? id)
    {
        
        var Team = await uow.GetRepository<Team>()
            .GetAllNonDeleted(x => x.Id == id)
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);
        if (Team == null)
        {
            return new TeamDto
            {
                ResponseStatus = DBResult.NotFound
            };
        }
        var UserTeam=await uow.GetRepository<UserTeam>().GetAllNonDeleted(x => x.TeamId == id)
            .ToListAsync() ;
        if(UserTeam.Count>0)
        {
            throw new BusinessException(ConstantKeys.ExceptionMessage.TeamExistsUserTeam);
        }
        uow.GetRepository<Team>().Delete(Team);
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
        team.DepartmentId = Team.DepartmentId;
        team.IsActive = Team.IsActive;
        team.UpdateById = userInfo.UserId;
        team.UpdateDate = DateTime.UtcNow;

        repository.Update(team);
        await uow.CommitAsync().ConfigureAwait(false);
        var result = mapper.Map<TeamDto>(team);
        result.ResponseStatus = DBResult.Updated;
        return result;
    }

    public async Task<UserTeamScopeDTO> UpdateUserTeamScope(UserTeamScopeDTO message)
    {
        if (message == null)
        {
            throw new BusinessException(ConstantKeys.ExceptionMessage.NoDataFound);
        }
        if (message.TeamId == null)
        {
            throw new BusinessException(ConstantKeys.ExceptionMessage.NoDataFound);
        }
        if (message.UserId == null)
        {
            throw new BusinessException(ConstantKeys.ExceptionMessage.NoDataFound);
        }
        if (message.ScopeId == null)
        {
            throw new BusinessException(ConstantKeys.ExceptionMessage.NoDataFound);
        }

        var dataexist = await uow.GetRepository<UserTeam>()
            .GetAllNonDeleted()
            .FirstOrDefaultAsync(x => x.Id==message.Id);
        UserTeam userteamobj= new UserTeam();
        if (dataexist == null)
        {
            userteamobj.TeamId = message.TeamId;
            userteamobj.UserId = message.UserId;
            userteamobj.IsActive = message.IsActive;
            uow.GetRepository<UserTeam>().Insert(userteamobj);

        }
        else
        {
            userteamobj = await uow.GetRepository<UserTeam>()
        .GetAllNonDeleted()
        .Where(x => x.Id == dataexist.Id).FirstAsync();

            userteamobj.IsActive = message.IsActive;
            uow.GetRepository<UserTeam>().Update(userteamobj);
        }
        List<UserTeamScope>  objUserTeamScopedelete = await uow.GetRepository<UserTeamScope>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.UserTeamId == userteamobj.Id)
                                      .ToListAsync();
        var UserTeamScopeexistids =new List<Guid>();
        if (objUserTeamScopedelete.Count > 0)
        {
            foreach (var item in objUserTeamScopedelete)
            {
                if (message.ScopeId.Contains(item.ScopeId))
                {
                    UserTeamScopeexistids.Add(item.ScopeId);
                }
                else
                {
                    uow.GetRepository<UserTeamScope>().Delete(item);
                }

            }
        }
        if (message.ScopeId != null)
        {
            var notInSelected = message.ScopeId.Except(UserTeamScopeexistids).ToList();
            List<UserTeamScope> objentitylist=new List<UserTeamScope>();
            foreach (var item in notInSelected)
            {
                UserTeamScope objentity = new UserTeamScope();
                objentity.UserTeamId = userteamobj.Id;
                objentity.ScopeId = item;
                objentity.IsActive = true;
                objentitylist.Add(objentity);
            }
            if (objentitylist.Count > 0)
            {
                await uow.GetRepository<UserTeamScope>().InsertRange(objentitylist);
            }


        }

        await uow.CommitAsync();
        message.Id = userteamobj.Id;
        message.ResponseStatus = DBResult.Updated;
        return message;
    }
    public async Task<UserTeamScopeDTO> DeleteUserTeamScopeTeamAsync(Guid? id)
    {
        UserTeamScopeDTO result=new UserTeamScopeDTO();
        var UserTeam = await uow.GetRepository<UserTeam>()
            .GetAllNonDeleted(x => x.Id == id)
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);
        if (UserTeam == null)
        {
            return new UserTeamScopeDTO
            {
                ResponseStatus = DBResult.NotFound
            };
        }
        var UserTeamScope = await uow.GetRepository<UserTeamScope>()
            .GetAllNonDeleted(x => x.UserTeamId == UserTeam.Id)
            .ToListAsync()
            .ConfigureAwait(false);
        uow.GetRepository<UserTeamScope>().DeleteRange(UserTeamScope);
        uow.GetRepository<UserTeam>().Delete(UserTeam);
        await uow.CommitAsync();
        result.ResponseStatus = DBResult.Deleted;
        return result;
    }
}

