using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.API.ScopeLayer;

public class ScopeRepostiory(IServiceScopeFactory serviceScopeFactory,
    CacheDataProvider cacheDataProvider,
    UnitOfWork unitOfWork,
    LoggingServices loggingServices,
    IMapper mapper,
    UserInfo userInfo,
    IServiceProvider serviceProvider,
    RequestInfo requestInfo
    ) : ApiBase(serviceScopeFactory, cacheDataProvider, unitOfWork, loggingServices, mapper, userInfo,
        serviceProvider, requestInfo)
{
    public async Task<List<ScopeAcademicYear>> GetScopeAcademicYearListByAcademicYearId(Guid academicYearId)
    {
        var list = await uow.GetRepository<ScopeAcademicYear>()
            .GetAllNonDeleted()
            .Include(x => x.Scope)
            .ThenInclude(x => x.ScopeType)
            .Include(x => x.ScopeParent)
            .Where(x=>x.AcademicYearId == academicYearId && x.DepartmentId == requestInfo.DepId)
            .ToListAsync();

        return list;
    }
}
