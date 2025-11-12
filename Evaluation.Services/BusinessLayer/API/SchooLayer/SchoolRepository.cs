using System.Threading.Tasks;
using Evaluation.DAL.Entities.Org;
using Evaluation.DAL.Entities.Planing;
using Evaluation.DAL.Helper;
using Evaluation.DAL.UnitOfWork;
using Evaluation.Services.Extensions;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.SchoolDto;
using Evaluation.SharedHelper.Helper;
using Evaluation.SharedHelper.Models;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.API.SchooLayer;

public class SchoolRepository(IServiceScopeFactory serviceScopeFactory,
    CacheDataProvider cacheDataProvider,
    UnitOfWork unitOfWork,
    LoggingServices loggingServices,
    IMapper mapper,
    UserInfo userInfo,
    IServiceProvider serviceProvider,
    RequestInfo requestInfo
    ) : ApiBase(serviceScopeFactory, cacheDataProvider, unitOfWork, loggingServices, userInfo,
        serviceProvider, requestInfo)
{
    public async Task<List<School>> GetSchoolsAsync()
    {
        var schools = await unitOfWork
            .GetRepository<School>()
            .GetAllActiveNonDeleted()
            .OrderByDescending(x => x.EstablishmentDate)
            .ToListAsync();
        return schools;
    }
    public async Task<List<SchoolVisits>> GetVisitTypes()
    {
        return await unitOfWork
            .GetRepository<VisitType>()
            .GetAllActiveNonDeleted()
            .ProjectToType<SchoolVisits>()
            .ToListAsync();
    }
}
