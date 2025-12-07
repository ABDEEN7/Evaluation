using System.Linq.Expressions;
using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Models.Planing;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Extensions;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.SchoolDto;
using Evaluation.SharedHelper.Extensions;
using Evaluation.SharedHelper.Helper;
using Evaluation.SharedHelper.Models;
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
    ) : ApiBase(serviceScopeFactory, cacheDataProvider, unitOfWork, loggingServices, mapper, userInfo,
        serviceProvider, requestInfo)
{
    public async Task<PaginatedResult<School>> GetSchoolsAsync(SchoolRequest request)
    {
        var filter = BuildFilterExpression(request);

        var query = serviceScopeFactory
             .CreateScopedUow()
             .GetRepository<School>()
             .GetAllNonDeleted(filter);
        return await query.GetPaginatedResult(request.PageNumber, request.PageSize = 10);
    }
    public IQueryable<VisitType> GetVisitTypes()
        => unitOfWork
            .GetRepository<VisitType>()
            .GetAllActiveNonDeleted();


    private Expression<Func<School, bool>> BuildFilterExpression(SchoolRequest request)
    {
        Expression<Func<School, bool>> filter = s => true;
        if (!string.IsNullOrWhiteSpace(request.Name))
            filter = filter.And(s => s.NameEn.Contains(request.Name) || s.NameAr.Contains(request.Name));
        if (request.EstablishmentDate != null)
        {
            int year = request.EstablishmentDate.Value.Year;
            filter = filter.And(s => s.EstablishmentDate.Year == year);
        }
        //if(request.VisitType != null)
        //    filter = filter.And(x=>x.)
        return filter;

    }
}