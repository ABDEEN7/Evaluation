using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Models.Planing.TeamsModule;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.API.TeamMemberBL;

public class TeamMemberService(IServiceScopeFactory serviceScopeFactory,
    CacheDataProvider cacheDataProvider,
    UnitOfWork unitOfWork,
    LoggingServices loggingServices,
    IMapper mapper,
    UserInfo userInfo,
    IServiceProvider serviceProvider,
    RequestInfo requestInfo
    ) : ApiBase(serviceScopeFactory, cacheDataProvider, unitOfWork, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
{
    public async Task<List<Team>> GetTeamAsync()
        => await unitOfWork.GetRepository<Team>()
            .GetAllActiveNonDeleted().ToListAsync();
    public async Task<List<Scope>> GetScopes()
    {
        Guid departmentId = await unitOfWork
    .GetRepository<Department>()
    .GetAllActiveNonDeleted(d =>
        d.UserDepartments.Any(ud => ud.UserId == userInfo.UserId))
    .OrderByDescending(x=>x.CreateDate)
    .Select(d => d.Id)
    .FirstOrDefaultAsync();

        return await unitOfWork.GetRepository<Scope>()
              //.GetAllActiveNonDeleted(x => x.DepartmentId == departmentId)
              .GetAllActiveNonDeleted()
              .ToListAsync();
    }
}
