using System.Threading.Tasks;
using Evaluation.DAL.Helper;
using Evaluation.Services.Extensions;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.SchoolDto;
using Evaluation.SharedHelper.Helper;
using Evaluation.SharedHelper.Models;
using Mapster;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Models.Planing;
using Evaluation.DAL.Repositories;
using Evaluation.SharedHelper.Consts;
using System.Linq.Expressions;

namespace Evaluation.Services.BusinessLayer.API.SchooLayer;

public class SchoolRepository(IServiceScopeFactory serviceScopeFactory,
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
    public IQueryable<School> GetSchools(Expression<Func<School, bool>>? filter = null)
          => unitOfWork
            .GetRepository<School>()
            .GetAllActiveNonDeleted(filter);

    public IQueryable<VisitType> GetVisitTypes()
        => unitOfWork
            .GetRepository<VisitType>()
            .GetAllActiveNonDeleted();

}
